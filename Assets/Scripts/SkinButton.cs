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
        try
        {
            data = MenuManager.Instance.GetSkinList[id];
        }
        catch (Exception e)
        {
            // recover from exception
        }
    }

    private void Refresh()
    {
        if (data != null)
        {
            image.sprite = data.mainTexture;
            image.color = data.unlocked ? Color.white : onLockColor;
        }
    }

    public void OnClick()
    {
        if (data.unlocked)
        {
            // TODO: move marker to the button's position
            MenuManager.Instance.SetSkin(data.mainTexture, data.onFireTexture);
            Transform marker = MenuManager.Instance.GetMark;
            marker.position = transform.position;
        }
        else
        {

        }
    }
}
