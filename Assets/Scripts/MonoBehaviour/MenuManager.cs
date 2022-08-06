using System.Collections.Generic;
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
    [SerializeField] private SkinsData skinsData;

    [SerializeField] private Text bestScore;
    [SerializeField] private Image playImage;
    [SerializeField] private Animator homeAnimator;
    [Header("Setting group")]
    [SerializeField] private SettingGroup settingGroup;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle vibrationToggle;
    [Header("Challenge group")]
    [SerializeField] private GameObject challengeGroup;
    [Header("Skin group")]
    [SerializeField] private GameObject skinGroup;
    [SerializeField] private Transform mark;

    public List<Skin> SkinList => skinsData.skins;
    public Transform GetMark => mark;
    private int skinId;
    public int SkinId => skinId;

    private void Start()
    {
        Application.targetFrameRate = 60;

        LoadPlayerSave();
        UpdatePlayButtonSkin();

        PlayIntro();
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

    public void SetSkin(int id)
    {
        skinId = id;
        PlayerPrefs.SetInt("Skin", id);
        Skin skin = skinsData.skins[id];
        playerSetting.mainSkin = skin.mainTexture;
        playerSetting.burnSkin = skin.onFireTexture;
        playerSetting.burnEffect = skin.burnEffect;
        playerSetting.flashColor = skin.flashColor;

        UpdatePlayButtonSkin();
    }

    public void PlayGame()
    {
        PlayGame(null);
    }

    public void PlayGame(Observer observer)
    {
        playerSetting.observer = observer;
        AudioManager.Instance.PlayGameAudio();
        SceneManager.LoadScene("Game");
    }

    private void PlayIntro()
    {
        string animationName = AudioManager.Instance.CheckFirstTime() ? "Open Game" : "Open Scene";
        homeAnimator.Play(animationName);
    }

    private void LoadPlayerSave()
    {
        int bestScore = PlayerPrefs.GetInt("HighScore", 0);
        this.bestScore.text = bestScore.ToString();

        skinId = PlayerPrefs.GetInt("Skin", 0);
        SetSkin(skinId);

        int volumeSetting = PlayerPrefs.GetInt("Volume", 1);
        AudioManager.Instance.SetVolume(volumeSetting > 0);

        int vibrateSetting = PlayerPrefs.GetInt("Vibration", 1);
        AudioManager.Instance.isVibrationOn = vibrateSetting > 0;
    }

    private void UpdatePlayButtonSkin()
    {
        playImage.sprite = playerSetting.mainSkin;
        playImage.SetNativeSize();
    }
}
