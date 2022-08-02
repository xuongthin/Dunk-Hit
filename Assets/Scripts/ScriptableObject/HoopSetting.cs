using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basketball hoop's setting", menuName = "Common setting for basketball hoop")]
public class HoopSetting : ScriptableObject
{
    public float lowestXPosition;
    public float highestXPosition;
    public float moveYAmount;
    [Header("")]
    public float getInDelay;
    public float getInTime;
    [Header("")]
    public float getOutDelay;
    public float getOutTime;
    [Header("Effects")]
    public float displayTime;
    public float fadeTime;
    public Sprite combo2;
    public Sprite combo4;
    public Sprite[] combo8;

}
