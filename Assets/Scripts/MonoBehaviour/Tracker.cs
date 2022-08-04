using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public static Tracker Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField] private SkinsData skinsData;
    private int[] trackValues;
    private string[] trackKeys;

    private void Start()
    {
        InitNLoad();
        CheckSkinsData();

        Debug.Log("Something");
    }

    private void InitNLoad()
    {
        int trackedDataTypeCount = Enum.GetNames(typeof(TrackedDataType)).Length;
        trackValues = new int[trackedDataTypeCount];
        trackKeys = new string[trackedDataTypeCount];
        for (int i = 0; i < trackedDataTypeCount; i++)
        {
            trackKeys[i] = "TrackData" + i.ToString();
            InitPref(out trackValues[i], trackKeys[i]);
        }
    }

    private void CheckSkinsData()
    {
        foreach (var skin in skinsData.skins)
        {
            if (!skin.unlocked)
                CheckCondition(skin);
        }
    }

    private void InitPref(out int value, string key)
    {
        value = PlayerPrefs.GetInt(key, -1);
        if (value < 0)
        {
            value = 0;
            PlayerPrefs.SetInt(key, value);
        }
    }

    private void CheckCondition(Skin skin)
    {
        int process = trackValues[((int)skin.conditionType)];
        skin.unlocked = process >= skin.condition;
    }
}

public enum TrackedDataType
{
    Jump,
    Score,
    BestScore,
    ScoreBasket,
    BestScoreBasket,
    Perfect,
    BestPerfectChain
}

