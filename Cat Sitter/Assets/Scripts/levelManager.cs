using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    // TODO: Multiple cats?
    public GameObject catRef;
    public List<Interactable> interactables;
    public GameObject interactablesContainer;

    // Start is called before the first frame update
    void Start()
    {
        // For every game object in Interactables, add it to the list of interactables
        // if it has an Interactable component
        foreach (Transform child in interactablesContainer.transform)
        {
            if (child.TryGetComponent<Interactable>(out var interactable))
            {
                interactables.Add(interactable);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO: Remove later

    public void debug_StartSinks()
    {
        foreach (Interactable interactable in interactables)
        {
            if (interactable is KitchenSinkController)
            {
                interactable.CatStart();
            }
        }
    }

    public void debug_FixSinks()
    {
        foreach (Interactable interactable in interactables)
        {
            if (interactable is KitchenSinkController)
            {
                interactable.PlayerFix();
            }
        }
    }

}
