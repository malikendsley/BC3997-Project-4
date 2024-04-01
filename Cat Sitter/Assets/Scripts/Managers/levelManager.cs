using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public ToolManager ToolManager { get; private set; }
    public UIController UIController { get; private set; }
    public ScreenRaycastManager ScreenRaycastManager { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            ToolManager = GetComponent<ToolManager>();
            UIController = GetComponent<UIController>();
            ScreenRaycastManager = GetComponent<ScreenRaycastManager>();
        }
    }


    // TODO: Multiple cats?
    public GameObject catRef;
    public List<Interactable> interactables;
    public GameObject interactablesContainer;

    public float cleanliness = 100; // TODO: Extract into stat block or something
    public float gameLength = 120;
    private float timeRemaining;

    enum GameState
    {
        Playing,
        Paused,
        GameEnd,
    }
    GameState gameState = GameState.Playing;

    // Start is called before the first frame update
    void Start()
    {
        // For every game object in Interactables, add it to the list of interactables
        // if it has an Interactable component
        // NOTE: Implicit requirement that all interactables' top level gameobject be the one to have the Interactable component
        foreach (Transform child in interactablesContainer.transform)
        {
            if (child.TryGetComponent<Interactable>(out var interactable))
            {
                interactables.Add(interactable);
            }
        }
        if (UIController == null)
        {
            Debug.LogError("UIController not set in LevelManager");
        }
        UIController.SetTime(gameLength);
        timeRemaining = gameLength;
        UIController.SetCleanliness(cleanliness);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.Playing:
                timeRemaining -= Time.deltaTime;
                UIController.SetTime(timeRemaining);
                if (timeRemaining <= 0)
                {
                    Debug.Log("Game Over");
                }
                break;
            case GameState.Paused:
                break;
            case GameState.GameEnd:
                break;
        }
    }


    // TODO: Remove later

    public void debug_StartSinks()
    {
        foreach (Interactable interactable in interactables)
        {
            if (interactable is KitchenSinkController)
            {
                interactable.CatActivateInteractable();
            }
        }
    }

    public void debug_TriggerBreakables()
    {
        foreach (Interactable interactable in interactables)
        {
            if (interactable is BreakableObjectController)
            {
                interactable.CatActivateInteractable();
            }
        }
    }

    public void debug_StartSummon()
    {
        foreach (Interactable interactable in interactables)
        {
            if (interactable is SummoningCircle)
            {
                interactable.CatActivateInteractable();
            }
        }
    }
}
