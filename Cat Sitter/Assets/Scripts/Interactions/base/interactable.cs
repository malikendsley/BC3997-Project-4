using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // TODO: Extract these into a stat block or something
    [SerializeField] protected float timeToCatastrophe;

    [SerializeField] protected float timeToFixActive;

    [SerializeField] protected float timeToFixCatastrophe;

    [SerializeField] protected float timeToCooldown;
    [SerializeField] protected Vector3 toolAnchorPoint;
    // Useful variables for inheriting classes
    protected float lastClickTime;
    protected bool playerInteracting;

    // TODO: Properly set up access to these from outside
    [SerializeField] float interactionDistance;
    [SerializeField] float interactionTime;
    // Idle: Nothing is happening
    // Active: The item is progressing towards a catastrophe
    // Catastrophe: The item is in the middle of a catastrophe
    // Cooldown: The item is in cooldown after a catastrophe
    public enum InteractionState { Idle, Active, Catastrophe, Cooldown }
    protected InteractionState state = InteractionState.Idle;
    bool reserved = false;
    [SerializeField] float cleanlinessTicker;
    [SerializeField] float cleanlinessTickTime = 1.0f;

    public float InteractionDistance { get => interactionDistance; set => interactionDistance = value; }
    public float InteractionTime { get => interactionTime; set => interactionTime = value; }
    public abstract void StartFixActive(); // Called when the player starts fixing the activated object
    public abstract void CancelFixActive(); // Called when the player cancels fixing the activated object
    public abstract void FinishFixActive(); // Called when the player finishes fixing the activated object

    public abstract void StartFixCatastrophe(); // Called when the player starts fixing the catastrophe
    public abstract void CancelFixCatastrophe(); // Called when the player cancels fixing the catastrophe
    public abstract void FinishFixCatastrophe(); // Called when the player finishes fixing the catastrophe

    public abstract void CatActivateInteractable(); // Called when a cat interacts with the object
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

    public InteractableData GetInteractionPackage()
    {
        return new InteractableData(timeToCatastrophe, timeToFixActive, timeToFixCatastrophe, timeToCooldown, toolAnchorPoint);
    }

    public bool TryReserve()
    {
        // Only idle interactables can be reserved by cats
        if (state != InteractionState.Idle)
        {
            return false;
        }
        if (!reserved)
        {
            reserved = true;
        }
        return reserved;
    }

    public void Release()
    {
        reserved = false;
    }
}

public class InteractableData
{
    public float timeToCatastrophe;
    public float timeToFixActive;
    public float timeToFixCatatrophe;
    public float timeToCooldown;
    public Vector3 toolAnchorPoint;

    public InteractableData(float timeToCatastrophe, float timeToFixActive, float timeToFixCatatrophe, float timeToCooldown, Vector3 toolAnchorPoint)
    {
        this.timeToCatastrophe = timeToCatastrophe;
        this.timeToFixActive = timeToFixActive;
        this.timeToFixCatatrophe = timeToFixCatatrophe;
        this.timeToCooldown = timeToCooldown;
        this.toolAnchorPoint = toolAnchorPoint;
    }
}
