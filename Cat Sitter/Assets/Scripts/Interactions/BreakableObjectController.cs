using System;
using System.Collections.Generic;
using UnityEngine;

// Breakable objects can be broken by the cat
// When activated, the object will be knocked over
// When it hits the ground, it will break
// Once the player fixes it, a new one will sprout up in the original position

public class BreakableObjectController : Interactable
{
    public GameObject fragileObj;
    public GameObject brokenObject;
    private Rigidbody rb;
    float cdTimer = 0.0f;
    public Vector3 knockImpulse;
    Vector3 originalPosition;
    Vector3 originalRotation;
    Vector3 originalScale;
    GameObject brokenObjRef;
    [SerializeField] private CollisionCommunicator comm;
    [SerializeField] ParticleSystem dustCloud;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(fragileObj.transform.position, knockImpulse);
    }

    void OnEnable()
    {
        comm.Broken += BreakObject;
    }

    void OnDisable()
    {
        comm.Broken -= BreakObject;
    }

    public override void CatActivateInteractable()
    {
        rb.isKinematic = false;
        rb.AddForce(knockImpulse, ForceMode.Impulse);
        state = InteractionState.Active;
    }


    void Start()
    {
        if (!fragileObj.TryGetComponent(out rb))
        {
            Debug.LogError("Rigidbody not found on breakable object");
        }
        originalPosition = fragileObj.transform.position;
        originalRotation = fragileObj.transform.rotation.eulerAngles;
        originalScale = fragileObj.transform.localScale;
    }

    public override void Update()
    {
        base.Update();
        switch (state)
        {
            case InteractionState.Catastrophe:
                if (playerInteracting)
                {
                    if (Time.time - lastClickTime > timeToFixCatastrophe)
                    {
                        FinishFixCatastrophe();
                    }
                }
                break;
            case InteractionState.Cooldown:
                // Wait for the cooldown to finish
                cdTimer -= Time.deltaTime;
                if (cdTimer <= 0.0f)
                {
                    state = InteractionState.Idle;
                    cdTimer = 0.0f;
                }
                break;
        }
        toolAnchorPoint = fragileObj.transform.position;
    }


    private void BreakObject()
    {
        Debug.Log("Breaking object");
        var breakString = "p" + UnityEngine.Random.Range(1, 3);
        LevelManager.Instance.AudioManager.PlayAudio(breakString);
        state = InteractionState.Catastrophe;
        fragileObj.GetComponent<MeshRenderer>().enabled = false;
        brokenObjRef = Instantiate(brokenObject, fragileObj.transform.position, fragileObj.transform.rotation);
        brokenObjRef.transform.parent = fragileObj.transform.parent;
        if (brokenObjRef.TryGetComponent<OutlineReceiver>(out var outlineReceiver))
        {
            outlineReceiver.attachedInteractable = this;
        }
    }

    private void ResetObject()
    {
        Debug.Log("Resetting object");
        state = InteractionState.Idle;
        fragileObj.GetComponent<MeshRenderer>().enabled = true;
        rb.isKinematic = true;
        // Grow a new object in the original position
        fragileObj.transform.localScale = new Vector3(.01f, .01f, .01f);
        LeanTween.scale(fragileObj, originalScale, 0.5f); // TODO: Extract
        fragileObj.transform.SetPositionAndRotation(originalPosition, Quaternion.Euler(originalRotation));
        if (brokenObjRef != null)
        {
            LeanTween.scale(brokenObjRef, new Vector3(.01f, .01f, .01f), 0.5f).setOnComplete(() =>  // TODO: Extract
            {
                Destroy(brokenObjRef);
                brokenObjRef = null;
            });
        }
        comm.Reset();
    }

    // An active breakable object is one that's been knocked but hasn't been broken yet
    public override void StartFixActive()
    {
        rb.isKinematic = true;
        fragileObj.transform.rotation = Quaternion.identity;
        var peakAbove = new Vector3((fragileObj.transform.position.x + originalPosition.x) / 2, originalPosition.y + 1, (fragileObj.transform.position.z + originalPosition.z) / 2);
        LeanTween.move(fragileObj, peakAbove, 0.25f).setEaseOutQuad().setOnComplete(() =>
        {
            LeanTween.move(fragileObj, originalPosition, 0.25f).setEaseInQuad().setOnComplete(() =>
            {
                state = InteractionState.Idle;
            });
        });
    }

    public override void CancelFixActive()
    {
        return; // Catching a falling broken object is instant
    }

    public override void FinishFixActive()
    {
        return; // Catching a falling broken object is instant
    }

    public override void StartFixCatastrophe()
    {
        if (state != InteractionState.Catastrophe)
        {
            Debug.LogWarning("Trying to fix a catastrophe that isn't happening");
            return;
        }
        dustCloud.Play();
    }

    public override void CancelFixCatastrophe()
    {
        if (state != InteractionState.Catastrophe)
        {
            return;
        }
        StopClear();
    }

    public override void FinishFixCatastrophe()
    {
        StopClear();
        state = InteractionState.Cooldown;
        cdTimer = timeToCooldown;
        ResetObject();
    }

    private void StopClear()
    {
        dustCloud.Stop();
        dustCloud.Clear();
    }
}
