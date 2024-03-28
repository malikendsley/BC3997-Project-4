using UnityEngine;
// When interacted with by a cat, the kitchen sink will be turned on.
// After a few seconds, the kitchen sink will flood in front of it.


public class KitchenSinkController : Interactable
{
    float currentTimer = 0.0f;
    public GameObject waterPlane;
    public GameObject floodPlane;
    public Transform lowerWaterPos;
    public Transform upperWaterPos;

    public override void CatInterrupted()
    {
        currentState = InteractionState.Idle;
    }

    public override void CatStart()
    {
        currentState = InteractionState.Active;
        currentTimer = TimeToCatastrophe;
        waterPlane.GetComponent<MeshRenderer>().enabled = true;
    }

    public override void PlayerFix()
    {
        currentState = InteractionState.Idle;
        floodPlane.GetComponent<MeshRenderer>().enabled = false;
        waterPlane.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == InteractionState.Active)
        {
            currentTimer -= Time.deltaTime;

            // As the sink floods, water will start to pool in the sink
            var frac = (TimeToCatastrophe - currentTimer) / TimeToCatastrophe;
            waterPlane.transform.position = Vector3.Lerp(lowerWaterPos.position, upperWaterPos.position, frac);
            if (currentTimer <= 0.0f)
            {
                currentState = InteractionState.Catastrophe;
                floodPlane.GetComponent<MeshRenderer>().enabled = true;

                currentTimer = 0.0f;
                Debug.Log("Kitchen sink is flooding!");
            }
        }
    }

    public override void TriggerCatastrophe()
    {
        return;
    }
}