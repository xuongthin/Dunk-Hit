using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

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

    [SerializeField] private AudioSource backgroundAudio;
    [SerializeField] private AudioSource tapAudio;
    [SerializeField] private AudioSource jumpAudio;
    [SerializeField] private AudioSource scoreAudio;
    [SerializeField] private AudioClip backgroundAudioClip;
    [SerializeField] private AudioClip gameAudioClip;
    public bool isVibrationOn;

    public void SetVolume(bool setting)
    {
        float value = setting ? 1 : 0;
        backgroundAudio.volume = value;
        tapAudio.volume = value;
        jumpAudio.volume = value;
        scoreAudio.volume = value;
    }

    public void PlayGameAudio()
    {
        backgroundAudio.Stop();
        backgroundAudio.clip = gameAudioClip;
        backgroundAudio.Play();
    }

    public void PlayMenuAudio()
    {
        backgroundAudio.Stop();
        backgroundAudio.clip = backgroundAudioClip;
        backgroundAudio.Play();
    }

    public void PlayTapSound()
    {
        tapAudio.Play();
    }

    public void PlayJumpAudio()
    {
        jumpAudio.Play();
    }

    public void PlayScoreSound()
    {
        scoreAudio.Play();
    }

    public void SetVibration(bool setting)
    {
        isVibrationOn = setting;
    }

    public void Vibrate()
    {
        if (isVibrationOn)
            Handheld.Vibrate();
    }
}
