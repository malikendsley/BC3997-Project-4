using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        {typeof(SummoningCircle), CatTool.FireExtinguisher}, // TODO: Change to FlammableObjectController
        {typeof(KitchenSinkController), CatTool.Wrench},
        {typeof(BreakableObjectController), CatTool.Dustpan},
        {typeof(CatInteractionReceiver), CatTool.CatGrabber},
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
            if (selectedTool == CatTool.None)
            {
                currentTool = null;
                currentToolScript = null;
                return;
            }
            currentTool = Instantiate(GetCurrentToolObject(selectedTool), toolPosition, Quaternion.identity);
            currentToolScript = currentTool.GetComponent<Tool>();
            if (!currentToolScript.WorldTool)
            {
                currentTool.transform.position = new Vector3(-3.27f, 2.07f, -0.81f);
            }
        }
        LevelManager.Instance.UIController.SetToolText(selectedTool);
    }

    // Called by the screen raycaster when the player clicks in the world
    // TODO: Push responsibilities into the receivers
    public void HandleScreenAction(OutlineReceiver receiver, ScreenAction action)
    {
        switch (action)
        {
            // When trying to interact with an object
            case ScreenAction.ClickObject:
                var interactable = receiver.GetInteractable(); // TODO: Brittle
                Debug.Log("Interactable Retrieved: " + interactable.GetType().ToString() + " " + interactable.GetState().ToString());
                // Special case clicking on an active object
                // (Active objects can be fixed before they become catastrophes without a tool)
                if (interactable is CatInteractionReceiver && selectedTool == CatTool.CatGrabber)
                {
                    usingTool = true;
                    currentTool.GetComponent<Tool>().StartUseTool(interactable);
                    receiver.InteractStart();
                }
                if (interactable.GetState() == Interactable.InteractionState.Active)
                {
                    // Unequip the tool and interact like normal
                    usingTool = false;
                    HandleToolSelected(CatTool.None);
                    receiver.InteractStart();
                }
                if (selectedTool == toolMap[interactable.GetType()] && interactable.GetState() == Interactable.InteractionState.Catastrophe)
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
                if (currentTool != null)
                {
                    usingTool = false;
                    currentTool.GetComponent<Tool>().StopUseTool();
                }
                receiver.InteractEnd();

                break;

            // When trying to interact with the world
            case ScreenAction.ClickWorld:
                if (currentTool != null)
                {
                    usingTool = true;
                    currentTool.GetComponent<Tool>().StartUseTool();
                }
                break;
            case ScreenAction.ReleaseWorld:
                if (currentTool != null)
                {
                    usingTool = false;
                    currentTool.GetComponent<Tool>().StopUseTool();
                }
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