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
    private Sprite fireSkin;
    private ParticleSystem[] effects;

    private Vector2 _pausedVelocity;
    private float _pausedAngularVelocity;

    private bool lastBreath;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.Instance.OnPause += Froze;
        GameManager.Instance.OnResume += Defrost;
    }

    public void SetSkin(Sprite skin)
    {
        GetComponent<SpriteRenderer>().sprite = skin;
    }

    public void SetSkin(Sprite skin1, Sprite skin2, GameObject effectPrefab1, GameObject effectPrefab2, GameObject effectPrefab3)
    {
        spriteRenderer.sprite = skin1;
        mainSkin = skin1;
        fireSkin = skin2;
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
            int combo = GameManager.Instance.ComboCount;
            if (combo <= 3)
            {
                effects[combo - 1].Play();
                if (combo > 1)
                    effects[combo - 2].Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
            if (GameManager.Instance.IsOnBurn)
            {
                spriteRenderer.sprite = fireSkin;
                AudioManager.Instance.Vibrate();
            }
            return true;
        }
        else
        {
            GameManager.Instance.OnScore(false);
            spriteRenderer.sprite = mainSkin;
            foreach (var effect in effects)
            {
                if (effect.isPlaying)
                    effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            return true;
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

        foreach (ParticleSystem particle in effects)
        {
            if (particle.isPlaying)
                particle.Pause();
        }
    }

    private void Defrost()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = _pausedVelocity;
            rb.angularVelocity = _pausedAngularVelocity;
        }

        foreach (ParticleSystem particle in effects)
        {
            if (particle.isPaused)
                particle.Play();
        }
    }

    public void Jump()
    {
        if (!lastBreath)
        {
            AudioManager.Instance.PlayJumpAudio();
            float direction = GameManager.Instance.HoopInRight ? 1 : -1;
            rb.velocity = new Vector2(jumpForce.x * direction, jumpForce.y);
        }
    }
}
