using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // TODO: Extract these into a stat block or something
    [SerializeField] protected float timeToCatastrophe;

    [SerializeField] protected float timeToFixActive;

    [SerializeField] protected float timeToFixCatastrophe;

    [SerializeField] protected float timeToCooldown;

    // Useful variables for inheriting classes
    protected float lastClickTime;
    protected bool playerInteracting;
    // Idle: Nothing is happening
    // Active: The item is progressing towards a catastrophe
    // Catastrophe: The item is in the middle of a catastrophe
    // Cooldown: The item is in cooldown after a catastrophe
    // TODO: Might need -ing states for transitions between states
    public enum InteractionState { Idle, Active, Catastrophe, Cooldown }
    protected InteractionState state = InteractionState.Idle;

    public abstract void StartFixActive(); // Called when the player starts fixing the activated object
    public abstract void CancelFixActive(); // Called when the player cancels fixing the activated object
    public abstract void FinishFixActive(); // Called when the player finishes fixing the activated object

    public abstract void StartFixCatastrophe(); // Called when the player starts fixing the catastrophe
    public abstract void CancelFixCatastrophe(); // Called when the player cancels fixing the catastrophe
    public abstract void FinishFixCatastrophe(); // Called when the player finishes fixing the catastrophe

    public abstract void CatActivateInteractable(); // Called when a cat interacts with the object
    public abstract void CatInterrupt(); // Used by the cat AI when it is interrupted (the catastrophe is not completed)

    public virtual void OnInteractStart() // Called when the player clicks on the object
    {
        Debug.Log("Player started interacting with object");
        playerInteracting = true;
        lastClickTime = Time.time;
        if (state == InteractionState.Active)
        {
            StartFixActive();
        }
        else if (state == InteractionState.Catastrophe)
        {
            StartFixCatastrophe();
        }
    }

    public virtual void OnInteractEnd() // Called when the player releases the object
    {
        Debug.Log("Player stopped interacting with object");
        playerInteracting = false;
        if (state == InteractionState.Active)
        {
            CancelFixActive();
        }
        else if (state == InteractionState.Catastrophe)
        {
            CancelFixCatastrophe();
        }
    }

    public InteractionState GetState()
    {
        return state;
    }

    public InteractableTimings GetInteractionPackage()
    {
        return new InteractableTimings(timeToCatastrophe, timeToFixActive, timeToFixCatastrophe, timeToCooldown);
    }
}

public class InteractableTimings
{
    public float timeToCatastrophe;
    public float timeToFixActive;
    public float timeToFixCatatrophe;
    public float timeToCooldown;

    public InteractableTimings(float timeToCatastrophe, float timeToFixActive, float timeToFixCatatrophe, float timeToCooldown)
    {
        this.timeToCatastrophe = timeToCatastrophe;
        this.timeToFixActive = timeToFixActive;
        this.timeToFixCatatrophe = timeToFixCatatrophe;
        this.timeToCooldown = timeToCooldown;
    }
}
