using UnityEngine;

// When interacted with by a cat, the kitchen sink will be turned on.
// After a few seconds, the kitchen sink will flood in front of it.

public class KitchenSinkController : Interactable
{
    public float currentTimer = 0.0f;

    public override void CatInterrupted()
    {
        currentState = InteractionState.Idle;
    }

    public override void CatStart()
    {
        currentState = InteractionState.Active;
        currentTimer = timeToCatastrophe;
    }

    public override void PlayerFix()
    {
        currentState = InteractionState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == InteractionState.Active)
        {
            currentTimer -= Time.deltaTime;

            // As the sink floods, water will start to pool in the sink

            if (currentTimer <= 0.0f)
            {
                currentState = InteractionState.Catastrophe;
                currentTimer = 0.0f;
                Debug.Log("Kitchen sink is flooding!");
            }
        }
    }
}