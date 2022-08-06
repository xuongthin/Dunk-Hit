using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinPopUp : MonoBehaviour
{
    [SerializeField] private Image skinImage;
    [SerializeField] private Text skinChallenge;
    private Animator animator;

    private Queue<Skin> unlockedSkinThisRound;

    private const string SHOW = "Show";

    private void Start()
    {
        GameManager.Instance.OnChallengeComplete += Show;
        GameManager.Instance.OnChallengeFail += Show;
        GameManager.Instance.OnEndGame += Show;

        Tracker.Instance.OnUnlock += QueuePopUp;

        unlockedSkinThisRound = new Queue<Skin>();
        animator = GetComponent<Animator>();
    }

    public void QueuePopUp(Skin skin)
    {
        unlockedSkinThisRound.Enqueue(skin);
    }

    public void OnPopUpClose()
    {
        if (unlockedSkinThisRound.Count > 0)
            Show();
    }

    private void Show()
    {
        Skin skin = unlockedSkinThisRound.Dequeue();
        SetInfoIntoPopUp(skin);
        animator.Play(SHOW);
    }

    private void SetInfoIntoPopUp(Skin skin)
    {
        skinImage.SetSpriteAndResize(skin.mainTexture);
        skinChallenge.text = skin.challenge;
    }
}
