using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: State machine this?
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
    GameObject currentTool;
    Tool currentToolScript;
    Vector3 toolPosition = new(0, 0, 0);
    [SerializeField] Vector3 toolOffset = new(0, 0, 0);
    [SerializeField] Vector3 startingToolPosition = new(0, 0, 0);
    [SerializeField] float toolDistance = 1.0f; // From 0 to 1, representing the distance from the camera
    [SerializeField] float toolMoveSpeed = 1f;
    bool usingTool = false;
    [SerializeField] GameObject catGrabber;
    [SerializeField] GameObject laserPointer;
    [SerializeField] GameObject fireExtinguisher;
    [SerializeField] GameObject wrench;
    [SerializeField] GameObject dustpan;
    // used to see if you hold the right tool for the job
    // TODO: Make interactables responsible for choosing how to react to interactions
    readonly Dictionary<Type, CatTool> toolMap = new(){
        {typeof(Interactable), CatTool.FireExtinguisher}, // TODO: Change to FlammableObjectController
        {typeof(KitchenSinkController), CatTool.Wrench},
        {typeof(BreakableObjectController), CatTool.Dustpan},
    };


    void Start()
    {
        HandleToolSelected(CatTool.None);
        toolPosition = startingToolPosition;
    }


    void Update()
    {
        // Cast a ray from the camera to the mouse position
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Set the tool position to the fraction of the distance between the camera and the mouse
        if (Physics.Raycast(ray, out var hit, 1000))
        {
            var hitpos = hit.point;
            toolPosition = Vector3.Lerp(Camera.main.transform.position, hitpos, toolDistance);
        }
        // Set the tool position to the mouse position loosely
        if (currentTool != null && currentToolScript.WorldTool && !usingTool)
        {
            currentTool.transform.position = Vector3.MoveTowards(currentTool.transform.position, toolPosition, toolMoveSpeed * Time.deltaTime);
        }
    }

    public void HandleToolSelected(CatTool tool)
    {
        if (currentTool != null)
        {
            currentToolScript = null;
            Destroy(currentTool);
            currentTool = null;
        }
        if (selectedTool == tool)
        {
            selectedTool = CatTool.None;
        }
        else
        {
            selectedTool = tool;

            currentTool = Instantiate(GetCurrentToolObject(selectedTool), toolPosition, Quaternion.identity);
            currentToolScript = currentTool.GetComponent<Tool>();
        }
        LevelManager.Instance.UIController.SetToolText(selectedTool);
    }

    // Called by the screen raycaster when the player clicks in the world
    public void HandleScreenAction(OutlineReceiver receiver, ScreenAction action)
    {
        Debug.Log("ToolManager: HandleScreenAction");
        switch (action)
        {
            // When trying to interact with an object
            case ScreenAction.ClickObject:
                var interactable = receiver.GetInteractable(); // TODO: Brittle
                // Special case clicking on an active object
                // (Active objects can be fixed before they become catastrophes without a tool)
                if (interactable.GetState() == Interactable.InteractionState.Active)
                {
                    // Unequip the tool and interact like normal
                    usingTool = false;
                    HandleToolSelected(CatTool.None);
                    receiver.InteractStart();
                }
                if (selectedTool == toolMap[interactable.GetType()])
                {
                    usingTool = true;
                    currentTool.GetComponent<Tool>().StartUseTool(interactable);
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
                usingTool = false;
                currentTool.GetComponent<Tool>().StopUseTool();
                receiver.InteractEnd();
                break;

            // When trying to interact with the world
            case ScreenAction.ClickWorld:
                Debug.Log("Click world");
                usingTool = true;
                currentTool.GetComponent<Tool>().StartUseTool();
                break;
            case ScreenAction.ReleaseWorld:
                usingTool = false;
                currentTool.GetComponent<Tool>().StopUseTool();
                break;
        }
    }

    GameObject GetCurrentToolObject(CatTool tool)
    {
        switch (tool)
        {
            case CatTool.CatGrabber:
                return catGrabber;
            case CatTool.LaserPointer:
                return laserPointer;
            case CatTool.FireExtinguisher:
                return fireExtinguisher;
            case CatTool.Wrench:
                return wrench;
            case CatTool.Dustpan:
                return dustpan;
            default:
                Debug.LogError("Tool not found");
                return null;
        }
    }
}