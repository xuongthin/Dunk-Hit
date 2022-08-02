using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sheet;
    [SerializeField] private ParticleSystem perfectEffect;
    [SerializeField] private ParticleSystem burnEffect;

    public void Trigger()
    {
        int combo = GameManager.Instance.ComboCount;
        if (combo == 1)
        {
            spriteRenderer.sprite = sheet[0];
        }
        else if (combo == 2)
        {
            spriteRenderer.sprite = sheet[1];
        }
        else
        {
            spriteRenderer.sprite = sheet[Random.Range(2, sheet.Length)];
        }

        if (GameManager.Instance.IsOnBurn)
        {
            burnEffect.Play();
        }
    }

}
