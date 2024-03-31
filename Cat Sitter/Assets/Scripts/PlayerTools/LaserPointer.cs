using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : Tool
{
    bool on = false;

    void Update()
    {
        if (on)
        {

        }
    }

    public override void StartUseTool(Interactable interactable = null)
    {
        on = true;
    }

    public override void StopUseTool()
    {
        on = false;
    }
}
