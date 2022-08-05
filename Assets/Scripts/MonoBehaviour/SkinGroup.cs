using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkinGroup : MonoBehaviour
{
    [SerializeField] private float time;
    private Vector3 hidePosition;
    private CanvasGroup canvasGroup;
    [Header("Skin info group")]
    [SerializeField] private Image skinImage;
    [SerializeField] private Text skinChallenge;
    [SerializeField] private Text processText;
    [SerializeField] private Image processBar;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        hidePosition = new Vector3(0, -Screen.height, transform.position.z);
        rectTransform.anchoredPosition = hidePosition;

        gameObject.SetActive(true);
    }

    public void Show()
    {
        transform.DOLocalMove(Vector3.zero, time).OnComplete(() =>
        {
            canvasGroup.interactable = true;
        });
    }

    public void Hide()
    {
        canvasGroup.interactable = false;
        transform.DOLocalMove(hidePosition, time);
    }

    private void OnDrawGizmosSelected()
    {
        float scale = transform.lossyScale.x;
        Gizmos.DrawWireCube(hidePosition * scale, new Vector3(1080, 1920, 0) * scale);
    }

    private void InitButtons()
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
        skinImage.sprite = skin.mainTexture;
        skinChallenge.text = skin.challenge;

        int process = Tracker.Instance.GetData(((int)skin.conditionType));
        processText.text = process.ToString() + "/" + skin.condition.ToString();
        processBar.fillAmount = process / skin.condition;
    }
}
