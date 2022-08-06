using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "Player Setting")]
public class PlayerSetting : ScriptableObject
{
    public Sprite mainSkin;
    public Sprite burnSkin;
    public GameObject burnEffect;
    public Color flashColor;
    public Observer observer;
}
