using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge1 : Challenge
{
    [SerializeField] private int targetScore;

    protected override void Init()
    {
        GameManager.Instance.OnScore += delegate (bool value)
        {
            if (GameManager.Instance.Score >= targetScore)
            {
                GameManager.Instance.OnWin(this);
            }
        };
    }
}
