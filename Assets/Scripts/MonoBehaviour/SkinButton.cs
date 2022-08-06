using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkinButton : MonoBehaviour
{
    [SerializeField] private SkinButtonSetting setting;
    [SerializeField] private Image unseenMark;
    public Action<int> OnLockedClick;
    private Image image;
    private Button button;
    private Skin data;

    private int id;


    private void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        id = transform.GetSiblingIndex();
        if (MenuManager.Instance.SkinList.Count > id)
            data = MenuManager.Instance.SkinList[id];

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (data != null)
        {
            image.sprite = data.mainTexture;
            image.color = data.unlocked ? Color.white : setting.onLockedColor;
            unseenMark.enabled = data.unlocked && !data.looked;
        }
        else
        {
            button.interactable = false;
        }

        if (MenuManager.Instance.SkinId == id)
        {
            MarkSelf();
        }
    }

    public void OnClick()
    {
        AudioManager.Instance.PlayTapSound();
        if (data.unlocked)
        {
            MenuManager.Instance.SetSkin(id);
            MarkSelf();
            if (!data.looked)
            {
                data.looked = true;
                unseenMark.enabled = false;
                Tracker.Instance.SaveSkinLook(id);
            }
        }
        else
        {
            OnLockedClick(id);
        }
    }

    private void MarkSelf()
    {
        Transform marker = MenuManager.Instance.GetMark;
        marker.position = transform.position;
    }
}
