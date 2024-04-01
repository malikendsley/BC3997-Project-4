using UnityEngine;
using UnityEngine.AI;

public class CatController : MonoBehaviour
{

    [SerializeField] GameObject hat;
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    Vector3 destination = new(0, 0, 0);
    bool shouldMove = false;
    [SerializeField] float timeBetweenActivities = 4;
    [SerializeField] float activityTimerVariance = 1;
    float newActivityTimer = 0;
    float interactionTimer = 0;
    Interactable currentInteractable;
    float meowTimer = 0;
    float meowTime = 4;
    CatStates state = CatStates.Idle;

    enum CatStates
    {
        Idle,
        Walking,
        Sitting,
        InteractMove,
        InteractAct,
        Lockout,
    }

    void OnDrawGizmos()
    {
        if (destination != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(destination, 0.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        hat.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case CatStates.Idle:
                // do nothing
                break;
            case CatStates.Walking:
                shouldMove = true;
                break;
            case CatStates.Sitting:
                // do nothing, sit down
                animator.SetBool("sitting", true);
                break;
            case CatStates.InteractMove:
                // Walk over to the interactable and interact with it
                // Walking logic
                shouldMove = true;
                if (currentInteractable == null)
                {
                    Debug.LogError("Cat is in Interacting state but has no interactable assigned.");
                    state = CatStates.Idle;
                    break;
                }

                if (Vector3.Distance(transform.position, currentInteractable.transform.position) < currentInteractable.InteractionDistance)
                {
                    Debug.Log("Cat arrived at interactable.");
                    state = CatStates.InteractAct;
                    interactionTimer = currentInteractable.InteractionTime;
                    if (currentInteractable is SummoningCircle)
                    { // TODO: Better module communication for what animation to use
                        animator.SetTrigger("summoning");
                        hat.SetActive(true);
                    }
                    else
                    {
                        animator.SetTrigger("batting");
                    }
                }
                break;
            case CatStates.InteractAct:
                interactionTimer -= Time.deltaTime;
                if (interactionTimer <= 0)
                {
                    Debug.Log("Cat finished interacting with object.");
                    currentInteractable.CatActivateInteractable();
                    currentInteractable.Release();
                    hat.SetActive(false);
                    animator.SetTrigger("reset");
                    currentInteractable = null;
                    newActivityTimer = GetNewActivityTimer();
                    state = CatStates.Idle;
                }
                break;
        }

        if (shouldMove)
        {

            animator.SetBool("walking", agent.velocity.magnitude > 0.1f);
            agent.SetDestination(destination);
        }

        // Every so often, the cat will do something
        //TODO: Redesign to not require so many ifs
        if (newActivityTimer <= 0 && state != CatStates.Lockout && state != CatStates.InteractAct && state != CatStates.InteractMove)
        {
            newActivityTimer = GetNewActivityTimer();
            StartNewActivity();
        }
        else
        {
            newActivityTimer -= Time.deltaTime;
        }

        // Meow every so often
        if (meowTimer <= 0)
        {
            meowTimer = meowTime;
            var meowString = "meow" + Random.Range(1, 6);
            LevelManager.Instance.AudioManager.PlayAudio(meowString);
        }
        else
        {
            meowTimer -= Time.deltaTime;
        }
    }

    float GetNewActivityTimer()
    {
        return timeBetweenActivities + Random.Range(-activityTimerVariance, activityTimerVariance);
    }

    void StartNewActivity()
    {
        Debug.Log("Starting new activity.");
        // Activities available:
        // 10% - Sit down
        // 40% - Walk to a random point on the navmesh 
        // 50% - Interact with an interactable (or try to)
        var roll = Random.Range(0, 100);
        if (roll < 10)
        {
            Debug.Log("Cat switching to sitting.");
            state = CatStates.Sitting;
            animator.SetBool("sitting", true);
        }
        else if (roll < 50)
        {
            if (!LevelManager.Instance.GetRandomNavmeshPoint(out destination))
            {
                Debug.Log("Failed to get random navmesh point, doing something else instead.");
                StartNewActivity(); // Unlikely but possible
                return;
            } //TODO: Eliminate this coupling
            Debug.Log("Cat switching to walking. Destination: " + destination.ToString());
            state = CatStates.Walking;
            animator.SetBool("sitting", false);
            // Sample the navmesh for a random point and generate its path
        }
        else
        {
            Debug.Log("Cat switching to interact move.");
            currentInteractable = LevelManager.Instance.TryReserveInteractable();
            if (currentInteractable != null)
            {
                state = CatStates.InteractMove;
                // Walk to the closest part of navmesh to the interactable
                var success = NavMesh.SamplePosition(currentInteractable.transform.position, out NavMeshHit hit, 5.0f, NavMesh.AllAreas);
                if (!success)
                {
                    Debug.LogError("Failed to find a navmesh point near interactable " + currentInteractable.name + ", doing something else instead.");
                    state = CatStates.Idle;
                    return;
                }
                destination = hit.position;
                animator.SetBool("sitting", false);
            }
            else
            {
                Debug.Log("No interactables, doing something else instead.");
                StartNewActivity();
            }
        }
    }

    // Pause the state machine (such as when grabbed)
    // TODO: This would be better served by a PDA with real enter and exit blocks for states
    public void StartLockout()
    {
        Debug.Log("Cat is locked out.");
        state = CatStates.Lockout;
        hat.SetActive(false);
        animator.SetBool("sitting", false);
        animator.SetBool("walking", false);
        if (animator.GetBool("batting") || animator.GetBool("summoning"))
        {
            animator.SetTrigger("reset");
        }
        shouldMove = false;
        agent.ResetPath();
    }

    public void EndLockout()
    {
        Debug.Log("Cat is no longer locked out.");
        state = CatStates.Idle;
        animator.SetBool("sitting", true);
        shouldMove = false;
        newActivityTimer = GetNewActivityTimer();
    }
}
