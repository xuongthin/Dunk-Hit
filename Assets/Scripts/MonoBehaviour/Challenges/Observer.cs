using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer : ScriptableObject
{
    [HideInInspector] public int id;
    public string description;
    public bool hasTimeLimit;
    public float timeLimit;
    public abstract void Init();

    protected virtual void OnComplete()
    {
        Tracker.Instance.SaveChallengeComplete(id);
    }
}
