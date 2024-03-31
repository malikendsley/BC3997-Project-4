using System;
using UnityEngine;
// When interacted with by a cat, the kitchen sink will be turned on.
// After a few seconds, the kitchen sink will flood in front of it.


public class KitchenSinkController : Interactable
{
    public GameObject waterlevelFX;
    public GameObject floodFX;
    public Transform lowerWaterLevel;
    public Transform upperWaterLevel;
    private float currentTimer;
    [SerializeField] ParticleSystem dustCloud;
    private float cooldownTimer = 0.0f;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case InteractionState.Active:
                currentTimer -= Time.deltaTime;

                // As the sink floods, water will start to pool in the sink
                var frac = (timeToCatastrophe - currentTimer) / timeToCatastrophe;
                waterlevelFX.transform.position = Vector3.Lerp(lowerWaterLevel.position, upperWaterLevel.position, frac);
                if (currentTimer <= 0.0f && !playerInteracting) // Be forgiving if the player is interacting
                {
                    state = InteractionState.Catastrophe;
                    floodFX.GetComponent<MeshRenderer>().enabled = true;
                    currentTimer = 0.0f;
                    Debug.Log("Kitchen sink is flooding!");
                }

                if (playerInteracting)
                {
                    if (Time.time - lastClickTime > timeToFixActive)
                    {
                        FinishFixActive();
                    }
                }
                break;
            case InteractionState.Catastrophe:
                // TODO: Every so many seconds, tick down the cleanliness
                if (playerInteracting)
                {
                    if (Time.time - lastClickTime > timeToFixCatastrophe)
                    {
                        FinishFixCatastrophe();
                    }
                }
                break;
            case InteractionState.Cooldown:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0.0f)
                {
                    state = InteractionState.Idle;
                    cooldownTimer = 0.0f;
                }
                break;
        }
    }

    public override void CatActivateInteractable()
    {
        state = InteractionState.Active;
        currentTimer = timeToCatastrophe;
        waterlevelFX.GetComponent<MeshRenderer>().enabled = true;
    }

    public override void StartFixCatastrophe()
    {
        if (state != InteractionState.Catastrophe)
        {
            return;
        }
        // Start the animation for the player fixing the sink
        dustCloud.Play();
        Debug.Log("Player started fixing the sink catastrophe");
    }

    public override void CancelFixCatastrophe()
    {
        if (state != InteractionState.Catastrophe)
        {
            return;
        }
        // Stop the animation for the player fixing the sink
        dustCloud.Stop();
        Debug.Log("Player stopped fixing the sink catastrophe");
    }

    public override void FinishFixCatastrophe()
    {
        dustCloud.Stop();
        state = InteractionState.Cooldown;
        cooldownTimer = timeToCooldown;
        floodFX.GetComponent<MeshRenderer>().enabled = false;
        waterlevelFX.GetComponent<MeshRenderer>().enabled = false;
        Debug.Log("Player finished fixing the sink catastrophe");
    }

    public override void CatInterrupt()
    {
        state = InteractionState.Idle;
    }

    public override void StartFixActive()
    {
        if (state != InteractionState.Active)
        {
            return;
        }
        // Start the animation for the player fixing the sink
        dustCloud.Play();
        Debug.Log("Player started fixing the sink");
    }

    public override void CancelFixActive()
    {
        if (state != InteractionState.Active)
        {
            return;
        }
        // Stop the animation for the player fixing the sink
        stopClear();
        Debug.Log("Player stopped fixing the sink");
    }

    public override void FinishFixActive()
    {
        stopClear();
        state = InteractionState.Idle;
        Debug.Log("Player finished fixing the sink");
        waterlevelFX.GetComponent<MeshRenderer>().enabled = false;

    }

    private void stopClear()
    {
        dustCloud.Stop();
        dustCloud.Clear();
    }

}