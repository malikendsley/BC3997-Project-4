using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class interactable : MonoBehaviour
{
    [SerializeField]
    private bool catAllowed;
    [SerializeField]
    private bool playerAllowed;
    enum PlayerInteractionTrigger {
        click
    }
    [SerializeField]
    PlayerInteractionTrigger trigger;
    
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanCatInteract(){
        return catAllowed;
    }

    public bool CanPlayerInteract(){
        return playerAllowed;
    }

    public void ToggleCatInteract(){
        catAllowed = !catAllowed;
    }

    public void TogglePlayerInteract(){
        catAllowed = !catAllowed;
    }

    private void InteractorCanInteract(GameObject interactor){
        if (interactor.Tag)
    }

    public virtual void Interact(GameObject interactor){
        
    }    

}
