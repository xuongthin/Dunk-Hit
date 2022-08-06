using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Challenge 1", menuName = "Challenge/Challenge 1", order = 1)]
public class Challenge1 : Observer
{
    [SerializeField] private int target;

    public override void Init()
    {
        GameManager.Instance.OnScore += Check;
    }

    private void Check(bool combo)
    {
        int score = GameManager.Instance.Score;
        if (score >= target)
        {
            GameManager.Instance.OnChallengeComplete();
            OnComplete();
        }
    }
}
