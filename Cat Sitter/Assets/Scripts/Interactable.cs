using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float timeToCatastrophe { get; set; }
    public enum InteractionState { Idle, Active, Catastrophe, Cooldown }
    protected InteractionState currentState = InteractionState.Idle;
    public abstract void CatStart(); // Used by the cat AI to activate the catastrophe
    public abstract void CatInterrupted(); // Used by the cat AI when it is interrupted (the catastrophe is not completed)
    public abstract void PlayerFix(); // Used by the player to fix the catastrophe if it has completed
    // TODO: Player cancel? (for example, if the player starts fixing the catastrophe but then changes their mind)
}