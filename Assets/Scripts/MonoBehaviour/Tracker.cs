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
    private const string TRACK_DATA = "TrackData";
    private int challengeTrack;
    private const string CHALLENGE_TRACK = "ChallengeTrack";
    private int skinLookTrack;
    private const string SKIN_LOOK_TRACK = "SkinLookTrack";
    private int currentScore;
    private int currentScoreBasket;
    private int currentPerfectChain;

    public Action<Skin> OnUnlock;

    private void Start()
    {
        InitNLoad();
        CheckSkinsData(true);
    }

    public void Attach()
    {
        GameManager.Instance.OnScore += delegate (bool perfect)
        {
            currentScore = GameManager.Instance.Score;
            if (currentScore > trackValues[(int)TrackedDataType.BestScore])
                trackValues[(int)TrackedDataType.BestScore] = currentScore;

            trackValues[(int)TrackedDataType.TotalScoreBasket] += 1;
            currentScoreBasket += 1;
            if (currentScoreBasket > trackValues[(int)TrackedDataType.BestScoreBasket])
                trackValues[(int)TrackedDataType.BestScoreBasket] = currentScoreBasket;

            if (perfect)
            {
                trackValues[(int)TrackedDataType.TotalPerfect] += 1;
                currentPerfectChain++;
                if (currentPerfectChain > trackValues[(int)TrackedDataType.BestPerfectChain])
                    trackValues[((int)TrackedDataType.BestPerfectChain)] = currentPerfectChain;
            }
            else
                currentPerfectChain = 0;

            CheckSkinsData();
        };

        GameManager.Instance.OnEndGame += delegate ()
        {
            Save();
        };
    }

    public void AddTotalJumpCount()
    {
        trackValues[(int)TrackedDataType.TotalJump] += 1;
        CheckSkinsData();
    }

    public int GetData(int id)
    {
        if (id >= 0 && id < trackValues.Length)
            return trackValues[id];
        else
            return -1;
    }

    public int GetChallengeData()
    {
        return challengeTrack;
    }

    public void SaveChallengeComplete(int id)
    {
        challengeTrack = challengeTrack | (1 << id);
        PlayerPrefs.SetInt(CHALLENGE_TRACK, challengeTrack);
    }

    public bool CheckUnseenSkin()
    {
        foreach (Skin skin in skinsData.skins)
        {
            if (skin.unlocked && !skin.looked)
                return true;
        }
        return false;
    }

    public void SaveSkinLook(int id)
    {
        skinLookTrack = skinLookTrack | (1 << id);
        PlayerPrefs.SetInt(SKIN_LOOK_TRACK, skinLookTrack);
    }

    private void InitNLoad()
    {
        int trackedDataTypeCount = Enum.GetNames(typeof(TrackedDataType)).Length;
        trackValues = new int[trackedDataTypeCount];
        trackKeys = new string[trackedDataTypeCount];
        for (int i = 0; i < trackedDataTypeCount; i++)
        {
            trackKeys[i] = TRACK_DATA + i.ToString();
            InitPref(out trackValues[i], trackKeys[i], 0);
        }

        InitPref(out challengeTrack, CHALLENGE_TRACK, 0);
        InitPref(out skinLookTrack, SKIN_LOOK_TRACK, 1);
    }

    private void CheckSkinsData(bool onLoad = false)
    {
        int i = 0;
        foreach (Skin skin in skinsData.skins)
        {
            if (onLoad)
            {
                skin.unlocked = CheckCondition(skin);
                skin.looked = (skinLookTrack & (1 << i)) != 0;
                i++;
            }
            else if (!skin.unlocked)
            {
                if (CheckCondition(skin))
                {
                    OnUnlock(skin);
                    Save();
                }
            }
        }
    }

    private void Save()
    {
        int trackedDataTypeCount = Enum.GetNames(typeof(TrackedDataType)).Length;
        for (int i = 0; i < trackedDataTypeCount; i++)
        {
            PlayerPrefs.SetInt(trackKeys[i], trackValues[i]);
        }
    }

    private void InitPref(out int value, string key, int initValue)
    {
        value = PlayerPrefs.GetInt(key, -1);
        if (value < 0)
        {
            value = initValue;
            PlayerPrefs.SetInt(key, value);
        }
    }

    private bool CheckCondition(Skin skin)
    {
        int process = trackValues[((int)skin.conditionType)];
        bool check = process >= skin.condition;
        skin.unlocked = check;
        return check;
    }
}

public enum TrackedDataType
{
    TotalJump,
    // most jump?
    // TotalScore,
    BestScore,
    TotalScoreBasket,
    BestScoreBasket,
    TotalPerfect,
    BestPerfectChain
}

