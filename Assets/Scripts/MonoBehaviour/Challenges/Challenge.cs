using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Challenge : MonoBehaviour
{
    public string description;
    protected bool useTime;
    protected bool useBigScoreOnPerfect;

    private void Start()
    {
        Init();
    }

    protected abstract void Init();
}
