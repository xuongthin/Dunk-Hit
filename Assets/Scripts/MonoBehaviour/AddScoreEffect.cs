using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreEffect : MonoBehaviour
{
    [SerializeField] private Sprite perfectSprite;
    [SerializeField] private Sprite comboSprite;
    [SerializeField] private Sprite[] bigComboSprites;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Trigger(Vector3 position)
    {
        transform.position = position;

        int combo = GameManager.Instance.ComboCount;
        if (combo == 1)
        {
            spriteRenderer.sprite = perfectSprite;
        }
        else if (combo == 2)
        {
            spriteRenderer.sprite = comboSprite;
        }
        else if (combo >= 3)
        {
            int id = Random.Range(0, bigComboSprites.Length);
            spriteRenderer.sprite = bigComboSprites[id];
        }

        animator.Play("Main");
    }
}
