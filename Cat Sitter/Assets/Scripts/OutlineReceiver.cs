using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineReceiver : MonoBehaviour
{
    // Outlines are drawn for any object on the layer "Outlined Objects"
    // Recursively move all of the objects this object is a parent of to or from the "Outlined Objects" layer
    public virtual void EnableOutline()
    {
        Recursionhelper(transform, true);
    }

    public virtual void DisableOutline()
    {
        Recursionhelper(transform, false);
    }

    void Recursionhelper(Transform t, bool value)
    {
        t.gameObject.layer = value ? LayerMask.NameToLayer("Outlined Objects") : LayerMask.NameToLayer("Default");
        foreach (Transform child in t)
        {
            Recursionhelper(child, value);
        }
    }
}
