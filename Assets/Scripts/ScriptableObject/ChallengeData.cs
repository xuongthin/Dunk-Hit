using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Challenge Data", menuName = "Challenge Data")]
public class ChallengeData : ScriptableObject
{
    public List<Challenge> challenges;
}

[System.Serializable]
public class Challenge
{
    public Observer observer;
    public bool isComplete;
}