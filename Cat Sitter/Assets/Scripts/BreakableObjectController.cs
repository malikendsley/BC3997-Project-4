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
    float currentCDTimer = 0.0f;
    public Vector3 knockImpulse;
    Vector3 originalPosition;
    Vector3 originalRotation;
    Vector3 originalScale;
    GameObject brokenObjRef;
    [SerializeField]
    private collisionCommunicator collisionCommunicator;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(fragileObj.transform.position, knockImpulse);
    }

    public override void CatInterrupted()
    {
        currentState = InteractionState.Idle;
    }

    public override void CatStart()
    {
        return;
    }

    public override void TriggerCatastrophe()
    {
        currentState = InteractionState.Catastrophe;
        rb.isKinematic = false;
        rb.AddForce(knockImpulse, ForceMode.Impulse);
    }

    public override void PlayerFix()
    {
        ResetObject();
    }

    void OnEnable()
    {
        collisionCommunicator.Broken += BreakObject;
    }

    void OnDisable()
    {
        collisionCommunicator.Broken -= BreakObject;
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

    public void Update()
    {
        switch (currentState)
        {
            case InteractionState.Catastrophe:
                break;
            case InteractionState.Cooldown:
                // Wait for the cooldown to finish
                currentCDTimer -= Time.deltaTime;
                if (currentCDTimer <= 0.0f)
                {
                    // Reset the object
                    ResetObject();
                }
                break;
            case InteractionState.Idle:
                // Do nothing
                break;
            case InteractionState.Active:
                // Do nothing
                break;
        }
    }


    private void BreakObject()
    {
        fragileObj.SetActive(false);
        brokenObjRef = Instantiate(brokenObject, fragileObj.transform.position, fragileObj.transform.rotation);
        brokenObjRef.transform.parent = fragileObj.transform.parent;
    }

    private void ResetObject()
    {
        Debug.Log("Resetting object");
        currentState = InteractionState.Idle;
        fragileObj.SetActive(true);
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
    }
}
