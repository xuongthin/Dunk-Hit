using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Combo

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private LevelsSetting levelsSetting;
    [SerializeField] private PlayerSetting playerSetting;
    [SerializeField] private float playZone;
    [SerializeField] private Rect hoopSpawnZone;
    [SerializeField] private Hoop[] hoops;
    [SerializeField] private float timeDecreaseEachScore;
    [SerializeField] private float minTime;
    private bool hoopInRight;

    private int score;
    private float maxTime;
    private Level currentLevel;

    private float timer;
    private bool isPlaying;
    private bool usedSecondChance;
    private int comboCount;

    public float PlayZone => playZone;
    public bool HoopInRight => hoopInRight;
    public int Score => score;
    public float TimeRemainInPercent => timer / maxTime;
    public Level GetCurrentLevel => currentLevel;
    public int ComboCount => comboCount;
    public bool IsOnBurn => comboCount >= 3;

    public Action OnGameStart;
    public Action<bool> OnScore;
    public Action<bool> OnStateChange;
    public Action OnPause;
    public Action OnResume;
    public Action OnTimeOut;
    public Action OnRevive;
    public Action OnEndGame;

    private void Start()
    {
        OnGameStart += delegate ()
        {
            usedSecondChance = false;
            hoopInRight = true;

            currentLevel = levelsSetting.levels[0]; // temp

            maxTime = currentLevel.initTime;
            timer = maxTime;

            isPlaying = true;

            WakeHoopUp();
        };

        OnScore += delegate (bool combo)
        {
            if (combo)
            {
                comboCount += 1;
                score += comboCount >= 3 ? 8 : (comboCount == 2 ? 4 : 2);
            }
            else
            {
                score += 1;
                comboCount = 0;
            }
            hoopInRight = !hoopInRight;
            UpdateTimer();
            WakeHoopUp();
        };

        OnPause += delegate () { isPlaying = false; };

        OnResume += delegate () { isPlaying = true; };

        OnTimeOut += delegate () { isPlaying = false; };

        OnRevive += delegate ()
        {
            maxTime = (maxTime + currentLevel.initTime) / 2;
            timer = maxTime;
            isPlaying = true;
        };

        Ball.Instance.SetSkin(playerSetting.mainSkin, playerSetting.onFireSkin, playerSetting.effects[0], playerSetting.effects[1], playerSetting.effects[2]);
        StartCoroutine(Start(0.25f));
    }

    private IEnumerator Start(float delay)
    {
        yield return Yielders.Get(delay);
        OnGameStart();
    }

    private void UpdateTimer()
    {
        maxTime -= timeDecreaseEachScore;
        if (maxTime < minTime)
            maxTime = minTime;

        timer = maxTime;
    }

    private void Update()
    {
        if (isPlaying)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (!usedSecondChance)
                {
                    OnTimeOut();
                    usedSecondChance = true;
                }
                else
                {
                    OnEndGame();
                }
            }
        }
    }

    public void WakeHoopUp()
    {
        int id = hoopInRight ? 1 : 0;
        hoops[id].WakeUp();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(Vector3.right * playZone, Vector3.up * 10);
        Gizmos.DrawWireCube(-Vector3.right * playZone, Vector3.up * 10);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)hoopSpawnZone.center, hoopSpawnZone.size);
    }
#endif
}
