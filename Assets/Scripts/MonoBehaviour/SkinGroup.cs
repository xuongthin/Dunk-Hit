using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SkinGroup : MonoBehaviour
{
    [SerializeField] private Vector3 hidePosition;
    [SerializeField] private float time;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
}
