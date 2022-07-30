using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private Image playImage;
    [Header("Setting group")]
    [SerializeField] private GameObject settingGroup;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle vibrationToggle;
    [Header("Skin group")]
    [SerializeField] private GameObject skinGroup;
    [SerializeField] private Transform mark;

    public List<Skin> GetSkinList => skinsSetting.skins;
    public Transform GetMark => mark;


    private void Start()
    {
        playImage.sprite = playerSetting.mainSkin;
        playImage.SetNativeSize();
        soundToggle.isOn = playerSetting.isSoundOn;
        vibrationToggle.isOn = playerSetting.isVibrationOn;
    }

    public void SetSound(bool value)
    {
        playerSetting.isSoundOn = value;
    }

    public void SetVibration(bool value)
    {
        playerSetting.isVibrationOn = value;
    }

    public void SetSkin(Sprite main, Sprite onFire)
    {
        playerSetting.mainSkin = main;
        playerSetting.onFireSkin = onFire;

        playImage.sprite = main;
        playImage.SetNativeSize();
    }

    public void ShowSettingGroup()
    {
        settingGroup.SetActive(true);
    }

    public void ShowSkinGroup()
    {
        skinGroup.SetActive(true);
    }

    public void Back2Main()
    {
        settingGroup.SetActive(false);
        skinGroup.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
}
