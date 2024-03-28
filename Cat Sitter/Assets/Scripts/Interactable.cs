using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    private float timeToCatastrophe;
    public float TimeToCatastrophe
    {
        get { return timeToCatastrophe; }
        set { timeToCatastrophe = value; }
    }

    [SerializeField]
    private float timeToFix;
    public float TimeToFix
    {
        get { return timeToFix; }
        set { timeToFix = value; }
    }

    [SerializeField]
    private float timeToCooldown;
    public float TimeToCooldown
    {
        get { return timeToCooldown; }
        set { timeToCooldown = value; }
    }

    public enum InteractionState { Idle, Active, Catastrophe, Cooldown }
    protected InteractionState currentState = InteractionState.Idle;
    public abstract void CatStart(); // Used by the cat AI to activate the catastrophe. Used for deferred catastrophes.
    public abstract void CatInterrupted(); // Used by the cat AI when it is interrupted (the catastrophe is not completed)
    public abstract void PlayerFix(); // Used by the player to fix the catastrophe if it has completed

    public abstract void TriggerCatastrophe(); // Cat can call this to signal exactly when the catastrophe should occur. Not every object will react to this.
    // TODO: Player cancel? (for example, if the player starts fixing the catastrophe but then changes their mind)
}