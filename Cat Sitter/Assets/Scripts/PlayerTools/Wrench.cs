using UnityEngine;

public class Wrench : Tool
{

    [SerializeField] Animator wrenchAnimator;
    float autoCancelTimer = 0.0f;
    float soundtimer;
    float soundfrequency = 0.75f;
    bool on = false;
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
        if (on)
        {
            if (soundtimer > 0)
            {
                soundtimer -= Time.deltaTime;
                if (soundtimer <= 0)
                {
                    LevelManager.Instance.AudioManager.PlayAudio("wrench");
                    soundtimer = soundfrequency;
                }
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
        transform.position = interactableData.toolAnchorPoint + interactable.transform.position;
        wrenchAnimator.SetBool("Wrenching", true);
        // If the interactable has a time to fix catastrophe, use that as the auto-cancel timer
        // This way, the tool stops sweeping when the catastrophe is fixed even if the player doesn't release the button
        autoCancelTimer = interactableData.timeToFixCatatrophe;
        on = true;
        soundtimer = soundfrequency;
    }

    public override void StopUseTool()
    {
        // The tool manager will reposition the tool on release
        wrenchAnimator.SetBool("Wrenching", false);
        on = false;
    }
}
