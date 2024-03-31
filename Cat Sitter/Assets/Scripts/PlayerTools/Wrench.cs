using UnityEngine;

public class Wrench : Tool
{

    [SerializeField] Animator wrenchAnimator;
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
        transform.position = interactableData.toolAnchorPoint;
        wrenchAnimator.SetBool("Wrenching", true);
        // If the interactable has a time to fix catastrophe, use that as the auto-cancel timer
        // This way, the tool stops sweeping when the catastrophe is fixed even if the player doesn't release the button
        autoCancelTimer = interactableData.timeToFixCatatrophe;
    }

    public override void StopUseTool()
    {
        // The tool manager will reposition the tool on release
        wrenchAnimator.SetBool("Wrenching", false);
    }
}
