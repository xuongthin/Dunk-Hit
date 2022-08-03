﻿using System.Collections.Generic;
using UnityEngine;
using DG;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private PlayerSetting playerSetting;
    [SerializeField] private SkinsSetting skinsSetting;

    [SerializeField] private Text highScore;
    [SerializeField] private Image playImage;
    [SerializeField] private Animator homeAnimator;
    [Header("Setting group")]
    [SerializeField] private SettingGroup settingGroup;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle vibrationToggle;
    [Header("Skin group")]
    [SerializeField] private GameObject skinGroup;
    [SerializeField] private Transform mark;

    public List<Skin> GetSkinList => skinsSetting.skins;
    public Transform GetMark => mark;
    private int skinId;
    public int SkinId => skinId;


    private void Start()
    {
        Application.targetFrameRate = 60;

        LoadPlayerSave();
        SetPlayButtonSkin();

        PlayIntro();
    }

    private void PlayIntro()
    {
        string animationName = AudioManager.Instance.CheckFirstTime() ? "Open Game" : "Open Scene";
        homeAnimator.Play(animationName);
    }

    private void SetPlayButtonSkin()
    {
        playImage.sprite = playerSetting.mainSkin;
        playImage.SetNativeSize();
    }

    private void LoadPlayerSave()
    {
        int bestScore = PlayerPrefs.GetInt("High Score", 0);
        highScore.text = bestScore.ToString();

        skinId = PlayerPrefs.GetInt("Skin", 0);
        SetSkin(skinId);

        int volumeSetting = PlayerPrefs.GetInt("Volume", 1);
        AudioManager.Instance.SetVolume(volumeSetting > 0);

        int vibrateSetting = PlayerPrefs.GetInt("Vibration", 1);
        AudioManager.Instance.isVibrationOn = vibrateSetting > 0;
    }

    public void SetSound(bool value)
    {
        AudioManager.Instance.SetVolume(value);
        PlayerPrefs.SetInt("Volume", value ? 1 : 0);
    }

    public void SetVibration(bool value)
    {
        AudioManager.Instance.isVibrationOn = value;
        PlayerPrefs.SetInt("Vibration", value ? 1 : 0);
    }

    public void SetSkin(Sprite main, Sprite onFire)
    {
        playerSetting.mainSkin = main;
        playerSetting.onFireSkin = onFire;

        playImage.sprite = main;
        playImage.SetNativeSize();
    }

    public void SetSkin(int id)
    {
        skinId = id;
        PlayerPrefs.SetInt("Skin", id);
        playerSetting.mainSkin = skinsSetting.skins[id].mainTexture;
        playerSetting.onFireSkin = skinsSetting.skins[id].onFireTexture;
        playerSetting.effects = new List<GameObject>();
        playerSetting.effects.Add(skinsSetting.skins[id].burnEffect1);
        playerSetting.effects.Add(skinsSetting.skins[id].burnEffect2);
        playerSetting.effects.Add(skinsSetting.skins[id].burnEffect3);

        SetPlayButtonSkin();
    }

    public void PlayGame()
    {
        AudioManager.Instance.PlayGameAudio();
        SceneManager.LoadScene("Game");
    }
}
