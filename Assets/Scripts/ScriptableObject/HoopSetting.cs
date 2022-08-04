using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basketball hoop's setting", menuName = "Common setting for basketball hoop")]
public class HoopSetting : ScriptableObject
{
    public float lowestXPosition;
    public float highestXPosition;
}
