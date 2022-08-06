using System;
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
    [SerializeField] private GameSetting gameSetting;
    [SerializeField] private float playZone;
    [SerializeField] private Hoop[] hoops;
    [SerializeField] private Animator flashEffect;
    [SerializeField] private Image flashImage;
    private bool hoopInRight;

    private int score;
    private float maxTime;

    private float timer;
    private bool isPlaying;
    private bool usedSecondChance;
    private int comboCount;
    private bool isChallenge;
    private bool hasTimeUpdate;

    public float PlayZone => playZone;
    public bool HoopInRight => hoopInRight;
    public int Score => score;
    public float TimeRemainInPercent => timer / maxTime;
    public int ComboCount => comboCount;
    public bool IsOnBurn => comboCount >= 3;

    public Action OnGameStart;
    public Action<bool> OnScore;
    public Action OnPause;
    public Action OnResume;
    public Action<Challenge> OnWin;
    public Action OnTimeOut;
    public Action OnChallengeComplete;
    public Action OnChallengeFail;
    public Action OnRevive;
    public Action OnEndGame;

    private void Start()
    {
        Application.targetFrameRate = 60;

        OnGameStart += delegate ()
        {
            hoopInRight = true;
            WakeHoopUp();
        };

        OnScore += CalculateScore;
        OnScore += delegate (bool combo)
        {
            isPlaying = true;

            if (hasTimeUpdate)
                UpdateTimer();

            hoopInRight = !hoopInRight;
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
            maxTime = (maxTime + gameSetting.initTime) / 2;
            timer = maxTime;
            isPlaying = true;
        };

        if (playerSetting.observer != null)
        {
            isChallenge = true;

            if (playerSetting.observer.hasTimeLimit)
            {
                hasTimeUpdate = false;
                InitTimer(playerSetting.observer.timeLimit);
            }
            else
            {
                hasTimeUpdate = true;
                InitTimer(0);
            }

            playerSetting.observer.Init();

            OnChallengeComplete += delegate ()
            {
                isPlaying = false;
                // TODO: Show UI
                Logger.Log("Success");
            };

            OnChallengeFail += delegate ()
            {
                isPlaying = false;
                // 
                Logger.Log("Fail");
            };
        }
        else
        {
            isChallenge = false;
            hasTimeUpdate = true;
            InitTimer(0);
        }

        isPlaying = false;
        usedSecondChance = false;

        Tracker.Instance.Attach();
        Ball.Instance.SetSkin(playerSetting.mainSkin, playerSetting.burnSkin, playerSetting.burnEffect);
        StartCoroutine(Start(0.25f));
    }

    public string GetChallengeText()
    {
        if (playerSetting.observer != null)
        {
            return playerSetting.observer.description;
        }

        return "";
    }

    public void ChallengeComplete()
    {

    }

    public void ChallengeFail()
    {

    }

    private IEnumerator Start(float delay)
    {
        yield return Yielders.Get(delay);
        OnGameStart();
    }

    private void InitTimer(float time)
    {
        maxTime = time > 0 ? time : gameSetting.initTime;
        timer = maxTime;
    }

    private void CalculateScore(bool combo)
    {
        if (combo)
        {
            comboCount += 1;
            score += comboCount >= 3 ? 8 : (comboCount == 2 ? 4 : 2);

            if (comboCount == 3)
            {
                flashImage.color = playerSetting.flashColor;
            }
            flashEffect.SetTrigger("On Score");

            if (comboCount >= 3)
                AudioManager.Instance.Vibrate();
        }
        else
        {
            score += 1;
            comboCount = 0;
            flashImage.color = Color.white;
        }
    }

    private void UpdateTimer()
    {
        maxTime -= gameSetting.timeReducePerScore;
        if (maxTime < gameSetting.minTime)
            maxTime = gameSetting.minTime;

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
        if (isChallenge)
        {
            OnChallengeFail();
            return;
        }

        comboCount = 0;

        if (usedSecondChance)
            OnEndGame();
        else
            OnTimeOut();
    }

}
