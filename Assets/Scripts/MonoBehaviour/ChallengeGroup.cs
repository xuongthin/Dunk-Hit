using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeGroup : MonoBehaviour
{
    [SerializeField] private ChallengeData data;
    [SerializeField] private Image buttonFillImage;
    [SerializeField] private float animationTime;
    private CanvasGroup canvasGroup;
    [SerializeField] private Image processBar;
    [SerializeField] private Text processText;
    [SerializeField] private ChallengeButtonSetting buttonSetting;
    [SerializeField] private Image[] challengeButtonImage;
    [SerializeField] private Text[] challengeTexts;

    private Vector3 hidePosition;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        InitPosition();
        LoadData();
        UpdateDisplay();
    }

    public void Show()
    {
        transform.DOLocalMove(Vector3.zero, animationTime).OnComplete(() =>
        {
            canvasGroup.interactable = true;
        });
    }

    public void PlayChallenge(int id)
    {
        data.challenges[id].observer.id = id;
        MenuManager.Instance.PlayGame(data.challenges[id].observer);
    }

    public void Hide()
    {
        canvasGroup.interactable = false;
        transform.DOLocalMove(hidePosition, animationTime);
    }

    private void InitPosition()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        hidePosition = new Vector3(0, -Screen.height, transform.position.z);
        rectTransform.anchoredPosition = hidePosition;
    }

    private void LoadData()
    {
        int complete = Tracker.Instance.GetChallengeData();
        for (int i = 0; i < data.challenges.Count; i++)
        {
            int bitMask = 1 << i;
            data.challenges[i].isComplete = (complete & bitMask) != 0;
        }
    }

    private void UpdateDisplay()
    {
        List<Challenge> challenges = data.challenges;
        int d = 0;
        for (int i = 0; i < challengeTexts.Length && i < challengeButtonImage.Length && i < challenges.Count; i++)
        {
            challengeTexts[i].text = challenges[i].observer.description;
            if (data.challenges[i].isComplete)
            {
                d++;
                challengeButtonImage[i].sprite = buttonSetting.completeSprite;
            }
            else
            {
                challengeButtonImage[i].sprite = buttonSetting.incompleteSprite;
                challengeTexts[i].color = buttonSetting.incompleteTextColor;
            }
        }

        processText.text = d.ToString() + "/" + challenges.Count.ToString();
        processBar.fillAmount = (float)d / challenges.Count;
        buttonFillImage.fillAmount = processBar.fillAmount;
    }
}
