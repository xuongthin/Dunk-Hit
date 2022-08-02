using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverGroup : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private GameObject main;
    [SerializeField] private Image timerBar;
    [SerializeField] private float time;
    [SerializeField] private float secondChanceTime;
    private float timer;

    private void Awake()
    {
        GameManager.Instance.OnTimeOut += Show;
    }

    public void Show()
    {
        main.SetActive(true);
        enabled = true;
        background.enabled = true;
        StartCoroutine(ShowBackground());
    }

    public void Hide()
    {
        enabled = false;
        main.SetActive(false);
        background.enabled = false;
    }

    private IEnumerator ShowBackground()
    {
        Color tempColor = background.color;
        float a = tempColor.a;
        float lerp = 0;
        while (lerp < time)
        {
            lerp += Time.deltaTime;
            tempColor.a = a * lerp / time;
            background.color = tempColor;
            yield return null;
        }
    }

    private void OnEnable()
    {
        timer = secondChanceTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        timerBar.fillAmount = timer / secondChanceTime;
        if (timer < 0)
        {
            GameManager.Instance.OnEndGame();
        }
    }

    public void GiveUp()
    {
        GameManager.Instance.OnEndGame();
    }
}
