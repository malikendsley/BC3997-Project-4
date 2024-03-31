using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    // TODO: Multiple cats?
    public GameObject catRef;
    public List<Interactable> interactables;
    public GameObject interactablesContainer;

    public float cleanliness = 100; // TODO: Extract into stat block or something
    public float gameLength = 120;
    private float timeRemaining;

    [SerializeField] UIController uicontroller;
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
        if (uicontroller == null)
        {
            Debug.LogError("UIController not set in LevelManager");
        }
        uicontroller.SetTime(gameLength);
        timeRemaining = gameLength;
        uicontroller.SetCleanliness(cleanliness);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.Playing:
                timeRemaining -= Time.deltaTime;
                uicontroller.SetTime(timeRemaining);
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
}
