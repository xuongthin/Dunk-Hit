using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinData", menuName = "Skin Data")]
public class SkinsData : ScriptableObject
{
    public List<Skin> skins;
}

[System.Serializable]
public class Skin
{
    public Sprite mainTexture;
    public Sprite onFireTexture;
    public GameObject burnEffect1;
    public GameObject burnEffect2;
    public GameObject burnEffect3;
    public bool unlocked;
    public string challenge;
    public TrackedDataType conditionType;
    public int condition;
}
