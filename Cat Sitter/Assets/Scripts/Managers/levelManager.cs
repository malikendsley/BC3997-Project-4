using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.GameCenter;


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

    public Vector3 roomBoundsCenter = new();
    public Vector3 roomBoundsExtents = new();

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(roomBoundsCenter, roomBoundsExtents * 2);
    }

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

    // TODO: Reserving interactables should probably be handled outside of them (like in here )
    public Interactable TryReserveInteractable()
    {
        System.Random rng = new();
        List<Interactable> shuffledInteractables = interactables.OrderBy(i => rng.Next()).ToList();

        foreach (Interactable i in shuffledInteractables)
        {
            if (i.TryReserve())
            {
                return i;
            }
        }
        return null;
    }


    public bool GetRandomNavmeshPoint(out Vector3 result)
    {
        for (int i = 0; i < 10; i++)
        {
            // Generate a random point within the bounds
            Vector3 randomPointInBounds = roomBoundsCenter + new Vector3(
                x: UnityEngine.Random.Range(-roomBoundsExtents.x, roomBoundsExtents.x),
                y: UnityEngine.Random.Range(-roomBoundsExtents.y, roomBoundsExtents.y),
                z: UnityEngine.Random.Range(-roomBoundsExtents.z, roomBoundsExtents.z)
            );

            if (NavMesh.SamplePosition(randomPointInBounds, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero; // A shorthand for new Vector3(0, 0, 0)
        return false;
    }

}
