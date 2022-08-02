using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SettingGroup : MonoBehaviour
{
    [SerializeField] private Transform popUp;
    [SerializeField] private Transform popUpStart;
    [SerializeField] private Vector3 popUpFinalPosition;
    [SerializeField] private float time;

    public void Show()
    {
        gameObject.SetActive(true);
        popUp.position = popUpStart.position;
        popUp.localScale = Vector3.zero;

        popUp.DOLocalMove(popUpFinalPosition, time);
        popUp.DOScale(Vector3.one, time);
    }

    public void Hide()
    {
        popUp.DOMove(popUpStart.position, time);
        popUp.DOScale(Vector3.zero, time).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
