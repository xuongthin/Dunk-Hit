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
    [Header("Skin group")]
    [SerializeField] private Image unseenSkinNotification;
    [SerializeField] private Transform mark;

    public List<Skin> SkinList => skinsData.skins;
    public Transform GetMark => mark;
    private int skinId;
    public int SkinId => skinId;

    private const string VOLUME = "Volume";
    private const string VIBRATION = "Vibration";
    private const string SKIN = "Skin";
    private const string OPEN_GAME = "Open Game";
    private const string OPEN_SCENE = "Open Scene";
    private const string GAME = "Game";

    private void Start()
    {
        Application.targetFrameRate = 60;

        LoadPlayerSave();
        UpdatePlayButtonSkin();
        UpdateNotification();
        PlayIntro();
    }

    public void SetSound(bool value)
    {
        AudioManager.Instance.SetVolume(value);
        PlayerPrefs.SetInt(VOLUME, value ? 1 : 0);
    }

    public void SetVibration(bool value)
    {
        AudioManager.Instance.isVibrationOn = value;
        PlayerPrefs.SetInt(VIBRATION, value ? 1 : 0);
    }

    public void UpdateNotification()
    {
        unseenSkinNotification.enabled = Tracker.Instance.CheckUnseenSkin();
    }

    public void SetSkin(int id)
    {
        skinId = id;
        PlayerPrefs.SetInt(SKIN, id);
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
        SceneManager.LoadScene(GAME);
    }

    private void PlayIntro()
    {
        string animationName = AudioManager.Instance.CheckFirstTime() ? OPEN_GAME : OPEN_SCENE;
        homeAnimator.Play(animationName);
    }

    private void LoadPlayerSave()
    {
        int bestScore = Tracker.Instance.GetData(((int)TrackedDataType.BestScore));
        this.bestScore.text = bestScore.ToString();

        skinId = PlayerPrefs.GetInt(SKIN, 0);
        SetSkin(skinId);

        int volumeSetting = Helper.GetPlayerPref(VOLUME, 1);
        AudioManager.Instance.SetVolume(volumeSetting > 0);
        soundToggle.isOn = volumeSetting > 0;

        int vibrateSetting = Helper.GetPlayerPref(VIBRATION, 1);
        AudioManager.Instance.isVibrationOn = vibrateSetting > 0;
        vibrationToggle.isOn = vibrateSetting > 0;
    }

    private void UpdatePlayButtonSkin()
    {
        playImage.sprite = playerSetting.mainSkin;
        playImage.SetNativeSize();
    }
}
