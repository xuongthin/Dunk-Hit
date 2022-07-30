using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "Player Setting")]
public class PlayerSetting : ScriptableObject
{
    public Sprite mainSkin;
    public Sprite onFireSkin;
    public List<GameObject> effects;
    public bool isSoundOn;
    public bool isVibrationOn;
}
