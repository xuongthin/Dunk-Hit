using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    private void Start()
    {
        InitMission();
    }

    protected abstract void InitMission();
}
