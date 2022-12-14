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
    public GameObject burnEffect;
    public Color flashColor;
    [HideInInspector] public bool unlocked;
    [HideInInspector] public bool looked;
    public string challenge;
    public TrackedDataType conditionType;
    public int condition;
}
