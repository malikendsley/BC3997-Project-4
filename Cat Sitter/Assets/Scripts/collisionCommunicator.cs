using UnityEngine;

public class CollisionCommunicator : MonoBehaviour
{
    public delegate void BrokenEvent();
    public event BrokenEvent Broken;
    public GameObject breakSurface;
    private bool broken = false;
    void OnCollisionEnter(Collision collision)
    {
        if (broken)
        {
            return;
        }
        print("Collision detected with " + collision.gameObject.name);
        if (collision.gameObject == breakSurface)
        {
            Broken();
            broken = true;
        }
    }

    public void Reset()
    {
        broken = false;
    }
}
