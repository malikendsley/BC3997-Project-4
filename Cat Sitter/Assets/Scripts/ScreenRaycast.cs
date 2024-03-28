using UnityEngine;

public class ScreenRaycast : MonoBehaviour
{

    OutlineReceiver lastHitObject;

    // Update is called once per frame
    void Update()
    {
        // cast ray from camera to mouse position
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // if it hits an object with an OutlineReceiver, enable the outline
        if (Physics.Raycast(ray, out var hit, 100))
        {
            if (hit.collider.TryGetComponent<OutlineReceiver>(out var outlineReceiver))
            {
                if (lastHitObject != outlineReceiver)
                {
                    if (lastHitObject != null)
                    {
                        lastHitObject.DisableOutline();
                    }
                    outlineReceiver.EnableOutline();
                    lastHitObject = outlineReceiver;
                }
            }
            // if it hits an object without an OutlineReceiver, disable the outline on the last object
            else
            {
                if (lastHitObject != null)
                {
                    lastHitObject.DisableOutline();
                    lastHitObject = null;
                }
            }
        }
        // if it hits an object without an OutlineReceiver, disable the outline on the last object
        else
        {
            if (lastHitObject != null)
            {
                lastHitObject.DisableOutline();
                lastHitObject = null;
            }
        }
    }
}
