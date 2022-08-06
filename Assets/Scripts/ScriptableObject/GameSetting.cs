using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Setting", menuName = "Game Setting")]
public class GameSetting : ScriptableObject
{
    public float initTime;
    public float timeReducePerScore;
    public float minTime;
}
