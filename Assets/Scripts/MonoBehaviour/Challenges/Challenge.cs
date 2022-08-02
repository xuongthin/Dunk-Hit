using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Challenge
{
    private void Start()
    {
        Init();
    }

    protected abstract void Init();
}
