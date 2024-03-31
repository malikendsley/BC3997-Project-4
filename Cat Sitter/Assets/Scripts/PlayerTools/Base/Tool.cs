using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public bool WorldTool = false;
    //TODO: Probably a separate class for world and non-world tools
    public float Cooldown = 0.0f;
    public abstract void StartUseTool(Interactable interactable = null);
    public abstract void StopUseTool();
}
