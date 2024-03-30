using UnityEngine;
// When interacted with by a cat, the kitchen sink will be turned on.
// After a few seconds, the kitchen sink will flood in front of it.


public class KitchenSinkController : Interactable
{
    float currentTimer = 0.0f;
    public GameObject waterlevelFX;
    public GameObject floodFX;
    public Transform lowerWaterLevel;
    public Transform upperWaterLevel;

    public override void CatInterrupted()
    {
        currentState = InteractionState.Idle;
    }

    public override void TriggerCatastrophe()
    {
        currentState = InteractionState.Active;
        currentTimer = TimeToCatastrophe;
        waterlevelFX.GetComponent<MeshRenderer>().enabled = true;
    }

    public override void PlayerFix()
    {
        currentState = InteractionState.Idle;
        floodFX.GetComponent<MeshRenderer>().enabled = false;
        waterlevelFX.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == InteractionState.Active)
        {
            currentTimer -= Time.deltaTime;

            // As the sink floods, water will start to pool in the sink
            var frac = (TimeToCatastrophe - currentTimer) / TimeToCatastrophe;
            waterlevelFX.transform.position = Vector3.Lerp(lowerWaterLevel.position, upperWaterLevel.position, frac);
            if (currentTimer <= 0.0f)
            {
                currentState = InteractionState.Catastrophe;
                floodFX.GetComponent<MeshRenderer>().enabled = true;

                currentTimer = 0.0f;
                Debug.Log("Kitchen sink is flooding!");
            }
        }
    }

    public override void OnInteractStart()
    {
        Debug.Log("Mouse pressed on " + gameObject.name + ".");
    }

    public override void OnInteractEnd()
    {
        Debug.Log("Mouse released on " + gameObject.name + ".");
    }
}