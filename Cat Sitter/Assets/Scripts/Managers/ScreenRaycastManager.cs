using UnityEngine;
using UnityEngine.InputSystem;

// Manages outlining valid moused over objects and sending clicks to them.
public enum ScreenAction
{
    ClickObject,
    ReleaseObject,
    ClickWorld,
    ReleaseWorld,
}


public class ScreenRaycastManager : MonoBehaviour
{
    OutlineReceiver selectedObject;
    OutlineReceiver clickedObject = null;
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Only update the outline if the mouse is not clicked
        if (Physics.Raycast(ray, out var hit, 100) && clickedObject == null)
        {
            // if an outline receiver is hit, enable its outline
            if (hit.collider.TryGetComponent<OutlineReceiver>(out var outlineReceiver))
            {
                if (selectedObject != outlineReceiver)
                {
                    if (selectedObject != null)
                    {
                        selectedObject.DisableOutline();
                    }
                    selectedObject = outlineReceiver;
                    selectedObject.EnableOutline();
                }
            }
            // otherwise, disable the outline of the previously selected object
            else
            {
                if (selectedObject != null)
                {
                    selectedObject.DisableOutline();
                    selectedObject = null;
                }
            }
        }
    }

    // This is a callback for the Input System, it is actually used.
    // TODO: Migrate to events
    private void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            // Check if the clicked object is an interactable
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100) && hit.collider.TryGetComponent<OutlineReceiver>(out var outlineReceiver))
            {
                clickedObject = outlineReceiver;
                LevelManager.Instance.ToolManager.HandleScreenAction(clickedObject, ScreenAction.ClickObject);
            }
            else
            {
                LevelManager.Instance.ToolManager.HandleScreenAction(null, ScreenAction.ClickWorld);
            }
        }
        else
        {
            // If the mouse is released, end the interaction if one was active
            if (clickedObject != null)
            {
                LevelManager.Instance.ToolManager.HandleScreenAction(clickedObject, ScreenAction.ReleaseObject);
                clickedObject = null;
            }
            else
            {
                LevelManager.Instance.ToolManager.HandleScreenAction(null, ScreenAction.ReleaseWorld);
            }
        }
    }

}
