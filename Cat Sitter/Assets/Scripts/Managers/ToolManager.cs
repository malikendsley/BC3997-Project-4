using System;
using System.Collections.Generic;
using UnityEngine;

public enum CatTool // TODO: Maybe scrap this system if it doesn't mature
{
    None, // Hand
    CatGrabber,
    LaserPointer,
    FireExtinguisher,
    Wrench,
    Dustpan,
}

public class ToolManager : MonoBehaviour
{

    public CatTool selectedTool = CatTool.None;

    // used to see if you hold the right tool for the job
    readonly Dictionary<Type, CatTool> toolMap = new(){
        {typeof(Interactable), CatTool.FireExtinguisher}, // TODO: Change to FlammableObjectController
        {typeof(KitchenSinkController), CatTool.Wrench},
        {typeof(BreakableObjectController), CatTool.Dustpan},
    };


    void Start()
    {
        HandleToolSelected(CatTool.None);
    }

    public void HandleToolSelected(CatTool tool)
    {
        if (selectedTool == tool)
        {
            selectedTool = CatTool.None;
        }
        else
        {
            selectedTool = tool;
        }
        LevelManager.Instance.UIController.SetToolText(selectedTool);
    }

    // Called by the screen raycaster when the player clicks on an interactable
    // Decide what to do based on the tool selected
    public void HandleScreenAction(OutlineReceiver receiver, ScreenAction action)
    {
        switch (action)
        {
            // When trying to interact with an object
            case ScreenAction.ClickObject:
                var interactable = receiver.GetInteractable(); // TODO: Brittle
                if (selectedTool == toolMap[interactable.GetType()])
                {
                    receiver.InteractStart();
                }
                else
                {
                    Debug.Log("Tool mismatch: " + selectedTool + " vs " + toolMap[interactable.GetType()]);
                }
                break;
            case ScreenAction.ReleaseObject:
                // Technically this would need the same validation but its very unlikely
                // to somehow swap tools without releasing the object
                receiver.InteractEnd();
                break;

            // When trying to interact with the world
            case ScreenAction.ClickWorld:
                // Depending on what tool you have, you'd use the tool
                break;
            case ScreenAction.ReleaseWorld:
                break;
        }
    }
}