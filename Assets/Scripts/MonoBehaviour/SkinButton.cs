using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    [SerializeField] private Color onLockColor;
    private Image image;

    private Skin data;

    private void Start()
    {
        image = GetComponent<Image>();
        int id = transform.GetSiblingIndex();
        if (MenuManager.Instance.GetSkinList.Count > id)
            data = MenuManager.Instance.GetSkinList[id];

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (data != null)
        {
            image.sprite = data.mainTexture;
            image.color = data.unlocked ? Color.white : onLockColor;
        }

        if (MenuManager.Instance.SkinId == transform.GetSiblingIndex())
        {
            Transform marker = MenuManager.Instance.GetMark;
            marker.position = transform.position;
        }
    }

    public void OnClick()
    {
        AudioManager.Instance.PlayTapSound();
        if (data.unlocked)
        {
            MenuManager.Instance.SetSkin(transform.GetSiblingIndex());
            Transform marker = MenuManager.Instance.GetMark;
            marker.position = transform.position;
        }
    }
}
