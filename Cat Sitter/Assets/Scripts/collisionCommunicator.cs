using UnityEngine;

public class CollisionCommunicator : MonoBehaviour
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
