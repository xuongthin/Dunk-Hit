using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Ball Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Transform ballCloneLeft;
    [SerializeField] private Transform ballCloneRight;
    [SerializeField] private Vector2 cloneOffset;
    [SerializeField] private Vector2 jumpForce;
    [SerializeField] private float minComboSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Sprite mainSkin;
    private Sprite burnSkin;
    [SerializeField] private ParticleSystem jumpEffect;
    [SerializeField] private ParticleSystem smokeEffect;
    [SerializeField] private ParticleSystem burnEffect;

    private Vector2 _pausedVelocity;
    private float _pausedAngularVelocity;
    private bool lastBreath;
    private int effectState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.Instance.OnPause += Froze;
        GameManager.Instance.OnResume += Defrost;
        GameManager.Instance.OnRevive += delegate ()
        {
            InitPositionNVelocity();
        };

        InitPositionNVelocity();
    }

    public void SetSkin(Sprite skin)
    {
        GetComponent<SpriteRenderer>().sprite = skin;
    }

    public void SetSkin(Sprite mainSkin, Sprite burnSkin, GameObject burnEffect)
    {
        this.mainSkin = mainSkin;
        this.burnSkin = burnSkin;
        spriteRenderer.sprite = mainSkin;
        ParticleSystem burnEffectController = Instantiate(burnEffect, transform).GetComponent<ParticleSystem>();
        this.burnEffect = burnEffectController;
    }

    public void SetTimeOut()
    {
        lastBreath = true;
    }

    private void Update()
    {
        if (rb.position.x > GameManager.Instance.PlayZone)
        {
            transform.position = ballCloneLeft.position;
        }

        if (rb.position.x < -GameManager.Instance.PlayZone)
        {
            transform.position = ballCloneRight.position;
        }

        ballCloneLeft.position = transform.position - (Vector3)cloneOffset;
        ballCloneRight.position = transform.position + (Vector3)cloneOffset;

        if (lastBreath && rb.position.y < -5)
        {
            lastBreath = false;
            GameManager.Instance.OfficialTimeOut();
            SetDisplay();
        }
    }

    public void Jump()
    {
        if (!lastBreath)
        {
            float direction = GameManager.Instance.HoopInRight ? 1 : -1;
            rb.velocity = new Vector2(jumpForce.x * direction, jumpForce.y);

            AudioManager.Instance.PlayJumpAudio();
            jumpEffect.Play();

            Tracker.Instance.AddTotalJumpCount();
        }
    }

    public void CheckCombo()
    {
        AudioManager.Instance.PlayScoreSound();
        lastBreath = false;

        // modify velocity so that it's easier to score next time
        if (Mathf.Sign(rb.velocity.x) * Mathf.Sign(rb.position.x) > 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (rb.velocity.sqrMagnitude > minComboSpeed * minComboSpeed)
        {
            GameManager.Instance.OnScore(true);
            SetDisplay();
        }
        else
        {
            GameManager.Instance.OnScore(false);
            SetDisplay();
        }
    }

    private void InitPositionNVelocity()
    {
        rb.position = new Vector2(0, 0);
        rb.velocity = new Vector2(0, 25);
    }

    private void SetDisplay()
    {
        int combo = GameManager.Instance.ComboCount;
        int newEffectState = Mathf.Clamp(combo, 0, 3);
        if (newEffectState != effectState)
        {
            if (newEffectState == 2)
            {
                spriteRenderer.sprite = mainSkin;
                smokeEffect.Play();
            }
            else if (newEffectState == 3)
            {
                spriteRenderer.sprite = burnSkin;
                StopEffect(smokeEffect);
                burnEffect.Play();
            }
            else if (effectState == 2)
            {
                StopEffect(smokeEffect);
            }
            else if (effectState == 3)
            {
                spriteRenderer.sprite = mainSkin;
                StopEffect(burnEffect);
            }

            effectState = newEffectState;
        }
    }

    private void StopEffect(ParticleSystem effect)
    {
        if (effect.isPlaying)
            effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    private void Froze()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            _pausedVelocity = rb.velocity;
            _pausedAngularVelocity = rb.angularVelocity;
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (effectState == 2)
            smokeEffect.Pause();
        else if (effectState == 3)
            burnEffect.Pause();
    }

    private void Defrost()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = _pausedVelocity;
            rb.angularVelocity = _pausedAngularVelocity;
        }

        if (effectState == 2)
            smokeEffect.Play();
        else if (effectState == 3)
            burnEffect.Play();
    }
}
