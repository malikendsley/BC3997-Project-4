using UnityEngine;

public class Dustpan : Tool
{

    [SerializeField] Animator dustpanAnimator;
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
            Debug.Log("Ignoring tool use: Dustpan requires an interactable");
            return;
        }
        var interactableData = interactable.GetInteractionPackage();
        transform.position = interactableData.toolAnchorPoint;
        dustpanAnimator.SetBool("Sweeping", true);
        // If the interactable has a time to fix catastrophe, use that as the auto-cancel timer
        // This way, the tool stops sweeping when the catastrophe is fixed even if the player doesn't release the button
        autoCancelTimer = interactableData.timeToFixCatatrophe;
    }

    public override void StopUseTool()
    {
        // The tool manager will reposition the tool on release
        dustpanAnimator.SetBool("Sweeping", false);
    }
}
