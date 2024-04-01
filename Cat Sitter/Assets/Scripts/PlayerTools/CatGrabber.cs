using UnityEngine;

public class CatGrabber : Tool
{
    [SerializeField] Animator grabberAnimator;
    CatInteractionReceiver cat;
    bool on = false;
    void Update()
    {
        if (on)
        {
            // Do the grabber thing
            if (cat)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    cat.transform.position = hit.point - ray.direction * 0.1f;
                    Debug.Log("Moving cat to " + cat.transform.position);
                    transform.position = cat.transform.position + cat.GetInteractionPackage().toolAnchorPoint;
                }
            }
            else
            {
                Debug.Log("No cat to grab");

            }
        }
    }

    public override void StartUseTool(Interactable interactable = null)
    {
        if (interactable == null || interactable is not CatInteractionReceiver)
        {
            return;
        }

        grabberAnimator.SetTrigger("Close");
        var interactableData = interactable.GetInteractionPackage();
        transform.position = interactableData.toolAnchorPoint + interactable.transform.position;
        cat = interactable as CatInteractionReceiver;
        cat.setColliderEnabled(false); // Prevent the raycast to align the cat from hitting the cat's own collider
        on = true;
        // If the interactable has a time to fix catastrophe, use that as the auto-cancel timer
        // This way, the tool stops sweeping when the catastrophe is fixed even if the player doesn't release the button
    }

    public override void StopUseTool()
    {
        // The tool manager will reposition the tool on release
        grabberAnimator.SetTrigger("Open");
        if (cat)
        {
            cat.setColliderEnabled(true);
        }
        cat = null;
        on = false;
    }
}
