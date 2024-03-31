// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TaskManager : MonoBehaviour
// {
//     [SerializeField]
//     private GameObject[] interactables;
//     [SerializeField]
//     private GameObject[] cats;

//     // Cats only go to objects that are untriggered
//     // these should be updated by the interactables calling functions in the task manager
//     private List<GameObject> untriggeredInteractors; 
//     private List<GameObject> triggeredInteractors;

//     private Dictionary<GameObject, Task> assignedTasks;
//     [SerializeField]
//     bool shareGoTriggers; // if more than one cat can have the same interactible target

//     // TASK TYPES
//     private Type[] TaskTypes = { typeof(Sit), typeof(Wander), typeof(GoTrigger) };
//     [SerializeField]
//     private float[] taskProbs;
//     // Holds the upper bound of the interval that the task gets triggered for.
//     // for example, if taskProbIntervals[0] = 0.1, TaskTypes[0] will get triggered when a number between 0 and 0.1
//     // is generated. Adding to the example, if taskProbIntervals[1] = 0.3, TaskTypes[1] will get triggered for vals
//     // between 0.1 and 0.3.
//     private float[] taskProbIntervals;

//     // Start is called before the first frame update
//     void Start()
//     {
//         Debug.Assert(TaskTypes.Length == TaskProbs.Length);
//         taskProbIntervals = new float[TaskTypes.Length];

//         // these should be updated by the interactables calling functions in the task manager
//         untriggeredObjs = interactables.ToList();
//         triggeredObjs = new List<int>();

//         assignedTasks = new Dictionary<GameObject, Task>(); // for debugging purposes

//         // Validate task probabilites
//         probSum = 0;
//         for (int i = 0; i < taskProbs.Length; ++i){
//             probSum += taskProbs[i];
//         }
//         if (probSum != 1){
//             Debug.Log("Probabilities don't sum to 1! Normalizing.");
//             for (int i = 0; i < taskProbs.Length; ++i){
//                 probSum[i] /= probSum;
//             }
//         }

//         // Create taskProbIntervals
//         float probSum = 0;
//         for (int i = 0; i < taskProbs.Length; ++i){
//             probSum += taskProbs[i];
//             taskProbIntervals[i] = probSum;
//         }

//         // Assign tasks to all cats
//         for (int i = 0; i < cats.size(); ++i){
//             NewTask(cat);
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         UpdateTasks();
//     }

//     void UpdateTasks(){
//         foreach (KeyValuePair<GameObject, Task> pair in assignedTasks){
//             pair.second.Update();
//         }
//     }

//     // Remove task from being assigned to cat
//     // Interrupts other cats doing the same task if applicable
//     // Method name is little misleading cause it also handles interrupt, 
//     // fix this if you have good naming sense
//     public void CompleteTask(GameObject cat){
//         assignedTasks.remove(cat);
//         // TODO: interrupt logic

//     }

//     int GenRandIdx(){
//         float r = UnityEngine.Random.Range(0,1);
//         int idx = 0;
//         while (taskProbIntervals[idx] > r){
//             ++idx;
//         }
//         return idx;
//     }

//     // Assign new task to cat
//     public void NewTask(GameObject cat){
//         // Use assigned probabilities to determine which task to assign
//         var taskType = TaskTypes[GenRandIdx()];
//         Task t = (Task)Activator.CreateInstance(taskType, cat);
//         assignedTasks[cat] = t;
//         t.Start();
//     }

//     // Called by Trigger Tasktype
//     public List<GameObject> GetAvailableInteractors() {
//         return untriggeredInteractors;
//     }

//     // Called by Trigger Tasktype
//     public bool ShareGoTriggers() {
//         return shareGoTriggers;
//     }

//     // Interrupt task assigned to cat
//     public void Interrupt(GameObject cat){

//     }
// }
