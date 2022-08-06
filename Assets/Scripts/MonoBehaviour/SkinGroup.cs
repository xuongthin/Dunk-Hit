using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkinGroup : MonoBehaviour
{
    [SerializeField] private Image buttonFillImage;
    [SerializeField] private float animationTime;
    private CanvasGroup canvasGroup;
    [SerializeField] private Text skinUnlockProcessText;
    [SerializeField] private Image skinUnlockProcessBar;
    [Header("Skin info group")]
    [SerializeField] private Animator skinInfoAnimator;
    [SerializeField] private Image skinImage;
    [SerializeField] private Text skinChallenge;
    [SerializeField] private Text processText;
    [SerializeField] private Image processBar;

    private Vector3 hidePosition;

    private const string OPEN = "Open";
    private const string HIDE = "Hide";

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        InitPosition();
        UpdateDisplay();
        LinkButtons();
    }

    public void Show()
    {
        transform.DOLocalMove(Vector3.zero, animationTime).OnComplete(() =>
        {
            canvasGroup.interactable = true;
        });
    }

    public void Hide()
    {
        canvasGroup.interactable = false;
        transform.DOLocalMove(hidePosition, animationTime);
    }

    public void HideSkinInfo()
    {
        skinInfoAnimator.Play(HIDE);
    }

    private void InitPosition()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        hidePosition = new Vector3(0, -Screen.height, transform.position.z);
        rectTransform.anchoredPosition = hidePosition;
    }

    private void OnDrawGizmosSelected()
    {
        float scale = transform.lossyScale.x;
        Gizmos.DrawWireCube(hidePosition * scale, new Vector3(1080, 1920, 0) * scale);
    }

    private void UpdateDisplay()
    {
        List<Skin> skins = MenuManager.Instance.SkinList;
        int d = 0;
        foreach (Skin skin in skins)
            if (skin.unlocked)
                d++;

        skinUnlockProcessText.text = d.ToString() + "/" + skins.Count.ToString();
        skinUnlockProcessBar.fillAmount = (float)d / skins.Count;
        buttonFillImage.fillAmount = (float)d / skins.Count;
    }

    private void LinkButtons()
    {
        SkinButton[] skinButtons = GetComponentsInChildren<SkinButton>();
        foreach (SkinButton button in skinButtons)
        {
            button.OnLockedClick += ShowSkinInfo;
        }
    }

    private void ShowSkinInfo(int id)
    {
        Skin skin = MenuManager.Instance.SkinList[id];
        skinImage.SetSpriteAndResize(skin.mainTexture);
        skinChallenge.text = skin.challenge;

        int process = Tracker.Instance.GetData(((int)skin.conditionType));
        processText.text = process.ToString() + "/" + skin.condition.ToString();
        processBar.fillAmount = (float)process / skin.condition;

        skinInfoAnimator.Play(OPEN);
    }
}
