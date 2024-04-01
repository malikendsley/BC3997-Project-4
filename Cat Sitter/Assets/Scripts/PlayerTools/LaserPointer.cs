
using UnityEngine;

public class LaserPointer : Tool
{
    [SerializeField] GameObject laserDot;
    [SerializeField] MeshRenderer laserRenderer;
    [SerializeField] float dotSize = 0.1f;
    bool on = false;
    void Start()
    {
        laserRenderer.enabled = false;
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.LookAt(laserDot.transform);
            // Align the rotation of the laser dot with the normal of the surface it hits
            // Move the laser dot slightly towards the camera to prevent z-fighting
            var adjustedPosition = hit.point - ray.direction * .1f;
            laserDot.transform.SetPositionAndRotation(adjustedPosition, Quaternion.FromToRotation(Vector3.up, hit.normal));
            // Scale the dot to have a consistent size on screen
            laserDot.transform.localScale = Camera.main.transform.position.y * dotSize * Vector3.one;
            if (on)
            {
                LevelManager.Instance.UIController.SetDist(hit.distance);
            }
        }
    }

    public override void StartUseTool(Interactable interactable = null)
    {
        laserRenderer.enabled = true;
        on = true;
    }

    public override void StopUseTool()
    {
        laserRenderer.enabled = false;
        on = false;
    }
}
