using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "Player Setting")]
public class PlayerSetting : ScriptableObject
{
    public bool openFirstTime;
    public Sprite mainSkin;
    public Sprite onFireSkin;
    public List<GameObject> effects;
}
