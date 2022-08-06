using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Challenge Button Setting", menuName = "Challenge Button Setting")]
public class ChallengeButtonSetting : ScriptableObject
{
    public Sprite incompleteSprite;
    public Color incompleteTextColor;
    public Sprite completeSprite;
}
