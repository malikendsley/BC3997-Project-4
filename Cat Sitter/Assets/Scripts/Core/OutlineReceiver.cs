using UnityEngine;

public class OutlineReceiver : MonoBehaviour
{
    // Outlines are drawn for any object on the layer "Outlined Objects"
    // Recursively move all of the objects this object is a parent of to or from the "Outlined Objects" layer
    // Since outlines and interactions are shared between objects, this script also routes clicks to interactables
    public Interactable attachedInteractable;
    public virtual void EnableOutline()
    {
        Recursionhelper(transform, true);
    }

    public virtual void DisableOutline()
    {
        Recursionhelper(transform, false);
    }

    // User clicks on the object
    public virtual void InteractStart()
    {
        attachedInteractable.OnInteractStart();
    }

    // User releases the click on the object
    public virtual void InteractEnd()
    {
        attachedInteractable.OnInteractEnd();
    }

    void Recursionhelper(Transform t, bool value)
    {
        t.gameObject.layer = value ? LayerMask.NameToLayer("Outlined Objects") : LayerMask.NameToLayer("Default");
        foreach (Transform child in t)
        {
            Recursionhelper(child, value);
        }
    }

    public virtual Interactable GetInteractable()
    {
        return attachedInteractable;
    }
}
