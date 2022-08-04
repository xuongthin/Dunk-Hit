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
    private ParticleSystem[] effects;
    private ParticleSystem currentEffect;

    private Vector2 _pausedVelocity;
    private float _pausedAngularVelocity;

    private bool lastBreath;

    public Action OnJump;

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

    public void SetSkin(Sprite skin1, Sprite skin2, GameObject effectPrefab1, GameObject effectPrefab2, GameObject effectPrefab3)
    {
        spriteRenderer.sprite = skin1;
        mainSkin = skin1;
        burnSkin = skin2;
        effects = new ParticleSystem[3];
        effects[0] = Instantiate(effectPrefab1, transform).GetComponent<ParticleSystem>();
        effects[1] = Instantiate(effectPrefab2, transform).GetComponent<ParticleSystem>();
        effects[2] = Instantiate(effectPrefab3, transform).GetComponent<ParticleSystem>();
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
            ChangeDisplay(-1);
        }
    }

    public void Jump()
    {
        if (!lastBreath)
        {
            AudioManager.Instance.PlayJumpAudio();
            float direction = GameManager.Instance.HoopInRight ? 1 : -1;
            rb.velocity = new Vector2(jumpForce.x * direction, jumpForce.y);
            OnJump();
        }
    }

    public bool CheckCombo()
    {
        if (rb.velocity.y > 0)
            return false;

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
            ChangeDisplay(GameManager.Instance.ComboCount - 1);
        }
        else
        {
            GameManager.Instance.OnScore(false);
            ChangeDisplay(-1);
        }
        return true;
    }

    private void InitPositionNVelocity()
    {
        rb.position = new Vector2(0, 0);
        rb.velocity = new Vector2(0, 25);
    }

    private void ChangeDisplay(int newEffectId)
    {
        if (currentEffect != null)
        {
            currentEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (newEffectId < 0)
        {
            currentEffect = null;
            spriteRenderer.sprite = mainSkin;
        }
        else
        {
            newEffectId = Mathf.Clamp(newEffectId, 0, effects.Length - 1);
            currentEffect = effects[newEffectId];
            currentEffect.Play();

            if (newEffectId >= 2)
            {
                spriteRenderer.sprite = burnSkin;
            }
        }
    }

    private void Froze()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            _pausedVelocity = rb.velocity;
            _pausedAngularVelocity = rb.angularVelocity;
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (currentEffect != null && currentEffect.isPlaying)
            currentEffect.Pause();
    }

    private void Defrost()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = _pausedVelocity;
            rb.angularVelocity = _pausedAngularVelocity;
        }

        if (currentEffect != null && currentEffect.isPaused)
            currentEffect.Play();
    }
}
