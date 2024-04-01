using UnityEngine;

public class Extinguisher : Tool
{
    [SerializeField] ParticleSystem p;
    float autoCancelTimer = 0.0f;

    void Update()
    {
        if (autoCancelTimer > 0)
        {
            autoCancelTimer -= Time.deltaTime;
            if (autoCancelTimer <= 0)
            {
                StopUseTool();
            }
        }
    }

    public override void StartUseTool(Interactable interactable = null)
    {
        if (interactable == null)
        {
            Debug.Log("Ignoring tool use: Extinguisher requires an interactable");
            return;
        }
        var interactableData = interactable.GetInteractionPackage();

        // Pick a random spot near the anchor point to move the tool to
        Vector3 randomOffset = RandomAnnulusPoint(2f, 3f, 1.5f);
        transform.position = interactableData.toolAnchorPoint + randomOffset + interactable.transform.position;
        // Face the tool towards the anchor point
        transform.LookAt(interactable.transform.position);
        p.Play();
        // If the interactable has a time to fix catastrophe, use that as the auto-cancel timer
        // This way, the tool stops sweeping when the catastrophe is fixed even if the player doesn't release the button
        autoCancelTimer = interactableData.timeToFixCatatrophe;
    }

    public override void StopUseTool()
    {
        // The tool manager will reposition the tool on release
        p.Stop();
    }

    private Vector3 RandomAnnulusPoint(float minRadius, float maxRadius, float height)
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        float radius = Mathf.Sqrt(Random.Range(minRadius * minRadius, maxRadius * maxRadius));
        return new Vector3(radius * Mathf.Cos(angle), height, radius * Mathf.Sin(angle));
    }
}
