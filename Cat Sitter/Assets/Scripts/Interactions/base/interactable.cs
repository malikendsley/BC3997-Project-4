using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class interactable : MonoBehaviour
{
    private bool catAllowed;
    private bool playerAllowed;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool CanCatInteract(){
        return catAllowed;
    }

    bool CanPlayerInteract(){
        return playerAllowed;
    }

    void ToggleCatInteract(){
        catAllowed = !catAllowed;
    }

    void TogglePlayerInteract(){
        catAllowed = !catAllowed;
    }

    override void Interact(GameObject interactor){

    }    

}
