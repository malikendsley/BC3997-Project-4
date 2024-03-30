using UnityEngine;
using UnityEngine.InputSystem;

// Manages outlining valid moused over objects and sending clicks to them.

public class ScreenRaycast : MonoBehaviour
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
    private void OnClick(InputValue value)
    {
        if (value.isPressed && selectedObject != null)
        {
            // Check if the clicked object is an interactable
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100))
            {
                if (hit.collider.TryGetComponent<OutlineReceiver>(out var outlineReceiver))
                {
                    clickedObject = outlineReceiver;
                    clickedObject.InteractStart();
                }
            }
        }
        else
        {
            // If the mouse is released, end the interaction if one was active
            if (clickedObject != null)
            {
                clickedObject.InteractEnd();
                clickedObject = null;
            }
        }
    }

}
