using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionCommunicator : MonoBehaviour
{
    public delegate void BrokenEvent();
    public event BrokenEvent Broken;
    public GameObject breakSurface;
    void OnCollisionEnter(Collision collision)
    {
        print("Collision detected with " + collision.gameObject.name);
        if (collision.gameObject == breakSurface)
        {
            Broken();
        }
    }
}
