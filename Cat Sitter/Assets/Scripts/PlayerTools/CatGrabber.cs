using UnityEngine;

public class CatGrabber : Tool
{
    [SerializeField] Animator grabberAnimator;

    public override void StartUseTool(Interactable interactable = null)
    {
        if (interactable == null)
        {
            return;
        }
        grabberAnimator.SetTrigger("Close");
        var interactableData = interactable.GetInteractionPackage();
        transform.position = interactableData.toolAnchorPoint;

        // If the interactable has a time to fix catastrophe, use that as the auto-cancel timer
        // This way, the tool stops sweeping when the catastrophe is fixed even if the player doesn't release the button
    }

    public override void StopUseTool()
    {
        // The tool manager will reposition the tool on release
        grabberAnimator.SetTrigger("Open");
    }
}
