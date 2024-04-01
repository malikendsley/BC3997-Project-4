using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: Distinguish interactables that are simply hooking into the click receiving system
public class CatInteractionReceiver : Interactable
{
    // CODE SMELL!!1!1 bruh
    public override void CancelFixActive()
    {
        throw new System.NotImplementedException();
    }

    public override void CancelFixCatastrophe()
    {
        throw new System.NotImplementedException();
    }

    public override void CatActivateInteractable()
    {
        throw new System.NotImplementedException();
    }

    public override void FinishFixActive()
    {
        throw new System.NotImplementedException();
    }

    public override void FinishFixCatastrophe()
    {
        throw new System.NotImplementedException();
    }

    public override void StartFixActive()
    {
        throw new System.NotImplementedException();
    }

    public override void StartFixCatastrophe()
    {
        throw new System.NotImplementedException();
    }

    [SerializeField] CatController catController;
    [SerializeField] Collider catCollider;

    // Successful interactions with the cat are only possible by the cat grabber,
    // so lock out the cat controller when the player is interacting with the cat
    public override void OnInteractEnd()
    {
        catController.EndLockout();
    }
    public override void OnInteractStart()
    {
        catController.StartLockout();
    }
    public void setColliderEnabled(bool enabled)
    {
        catCollider.enabled = enabled;
    }
}
