using UnityEngine;

public class OutlineSubReceiver : OutlineReceiver
{
    // This is attached to the child of an object with an OutlineReceiver.
    // It allows things like broken objects to behave as one object for the purposes of outlining and interaction.
    // Since broken objects are instantiated and destroyed, they have to register themselves with the parent object.
    OutlineReceiver parent;
    void Awake()
    {
        var searchedObject = transform.parent;
        while (parent == null && searchedObject) // TODO: Brittle.
        {
            parent = searchedObject.GetComponent<OutlineReceiver>();
            searchedObject = searchedObject.parent;
        }
        if (parent == null)
        {
            Debug.LogError("OutlineSubReceiver could not find parent OutlineReceiver");
        }
    }

    public override void EnableOutline()
    {
        parent.EnableOutline();
    }

    public override void DisableOutline()
    {
        parent.DisableOutline();
    }

    // User clicks on the object
    public override void InteractStart()
    {
        parent.InteractStart();
    }

    // User releases the click on the object
    public override void InteractEnd()
    {
        parent.InteractStart();
    }
}
