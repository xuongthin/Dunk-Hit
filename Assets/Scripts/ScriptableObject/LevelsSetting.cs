using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels Data", menuName = "Levels Data")]
public class LevelsSetting : ScriptableObject
{
    public List<Level> levels;
}

[System.Serializable]
public class Level
{
    public string description;
    public float initTime;
    public Challenge challenge;
}
