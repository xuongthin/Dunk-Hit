﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Hoop[] hoops;
    [SerializeField] private float timeDecreaseEachScore;
    [SerializeField] private float minTime;
    [SerializeField] private Animator flashEffect;
    [SerializeField] private Image flashImage;
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
    public Action OnPause;
    public Action OnResume;
    public Action<Challenge> OnWin;
    public Action OnTimeOut;
    public Action OnRevive;
    public Action OnEndGame;

    private void Start()
    {
        Application.targetFrameRate = 60;

        OnGameStart += delegate ()
        {
            usedSecondChance = false;
            hoopInRight = true;

            currentLevel = levelsSetting.levels[0]; // temp

            maxTime = currentLevel.initTime;
            timer = maxTime;

            isPlaying = false;

            WakeHoopUp();
        };

        OnScore += delegate (bool combo)
        {
            isPlaying = true;

            if (combo)
            {
                comboCount += 1;
                score += comboCount >= 3 ? 8 : (comboCount == 2 ? 4 : 2);

                if (comboCount == 3)
                {
                    flashImage.color = playerSetting.flashColor;
                }
                flashEffect.SetTrigger("On Score");
            }
            else
            {
                score += 1;
                comboCount = 0;
                flashImage.color = Color.white;
            }
            hoopInRight = !hoopInRight;
            UpdateTimer();
            WakeHoopUp();
        };

        OnPause += delegate () { isPlaying = false; };

        OnResume += delegate () { isPlaying = true; };

        OnTimeOut += delegate ()
        {
            isPlaying = false;
        };

        OnRevive += delegate ()
        {
            usedSecondChance = true;
            maxTime = (maxTime + currentLevel.initTime) / 2;
            timer = maxTime;
            isPlaying = true;
        };

        Tracker.Instance.Attach();
        Ball.Instance.SetSkin(playerSetting.mainSkin, playerSetting.burnSkin, playerSetting.burnEffect);
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
                Ball.Instance.SetTimeOut();
            }
        }
    }

    private void WakeHoopUp()
    {
        int id = hoopInRight ? 1 : 0;
        hoops[id].WakeUp();
    }

    public void OfficialTimeOut()
    {
        comboCount = 0;

        if (usedSecondChance)
            OnEndGame();
        else
            OnTimeOut();
    }

}
