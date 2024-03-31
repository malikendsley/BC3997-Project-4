using UnityEngine;

public class Extinguisher : Tool
{
    [SerializeField] ParticleSystem p;
    float autoCancelTimer = 0.0f;

    void Update()
    {
        if (autoCancelTimer > 0)
        {
            autoCancelTimer -= Time.deltaTime;
            if (autoCancelTimer <= 0)
            {
                StopUseTool();
            }
        }
    }

    public override void StartUseTool(Interactable interactable = null)
    {
        if (interactable == null)
        {
            Debug.Log("Ignoring tool use: Wrench requires an interactable");
            return;
        }
        var interactableData = interactable.GetInteractionPackage();

        // Pick a random spot near the anchor point to move the tool to
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        transform.position = interactableData.toolAnchorPoint + randomOffset;
        // Face the tool towards the anchor point
        transform.LookAt(interactableData.toolAnchorPoint);

        p.Play();
        // If the interactable has a time to fix catastrophe, use that as the auto-cancel timer
        // This way, the tool stops sweeping when the catastrophe is fixed even if the player doesn't release the button
        autoCancelTimer = interactableData.timeToFixCatatrophe;
    }

    public override void StopUseTool()
    {
        // The tool manager will reposition the tool on release
        p.Stop();
    }
}
