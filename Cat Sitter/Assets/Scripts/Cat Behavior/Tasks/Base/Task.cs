using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    private GameObject cat;

    void Task(GameObject cat_in){
        cat = cat_in;
    }

    // Start task
    public abstract void Start();

    // Update state, check for finish state
    public abstract void Update() {

    }

    // Task interrupted, finishes early without meeting end condition
    // eg. A cat should not try to trigger a sink if it is triggered by another cat
    public abstract void Interrupt() {

    }

    // Task pauses for whatever reason
    // eg. cat is picked up, distracted by toy? idk if that should be interrupt or pause
    public virtual void Pause() {

    }

    public virtual void Resume() {

    }

    // Finish task sequence
    public abstract void Finish() {
        TaskManager.CompleteTask(cat);
        TaskManager.NewTask(cat);
    }

}
