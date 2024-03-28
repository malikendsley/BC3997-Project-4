using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionCommunicator : MonoBehaviour
{
    public delegate void BrokenEvent();
    public static event BrokenEvent Broken;
    void OnCollisionEnter(Collision collision)
    {
        print("Collision detected with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Break Surface")
        {
            Broken();
        }
    }
}
