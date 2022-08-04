using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge1 : Challenge
{
    [SerializeField] private int targetScore;

    private int counter;

    protected override void Init()
    {
        counter = 0;

        GameManager.Instance.OnScore += delegate (bool value)
        {

        };
    }
}
