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
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Sprite mainSkin;
    private Sprite fireSkin;
    private ParticleSystem[] effects;

    private Vector2 _pausedVelocity;
    private float _pausedAngularVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameManager.Instance.OnPause += Froze;
        GameManager.Instance.OnResume += Defrost;
    }

    public void SetSkin(Sprite skin)
    {
        GetComponent<SpriteRenderer>().sprite = skin;

        ballCloneLeft.GetComponent<SpriteRenderer>().sprite = skin;
        ballCloneRight.GetComponent<SpriteRenderer>().sprite = skin;
    }

    public void SetSkin(Sprite skin1, Sprite skin2, GameObject effectPrefab1, GameObject effectPrefab2, GameObject effectPrefab3)
    {
        mainSkin = skin1;
        fireSkin = skin2;
        effects = new ParticleSystem[3];
        effects[0] = Instantiate(effectPrefab1, transform).GetComponent<ParticleSystem>();
        effects[1] = Instantiate(effectPrefab2, transform).GetComponent<ParticleSystem>();
        effects[2] = Instantiate(effectPrefab3, transform).GetComponent<ParticleSystem>();
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

        ballCloneLeft.rotation = transform.rotation;
        ballCloneRight.rotation = transform.rotation;
    }

    public void CheckCombo()
    {
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
        }
        else
        {
            GameManager.Instance.OnScore(false);
            foreach (var effect in effects)
            {
                if (effect.isPlaying)
                    effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        // modify velocity so that it's easier to score next time
        if (Mathf.Sign(rb.velocity.x) * Mathf.Sign(rb.position.x) > 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
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
        float direction = GameManager.Instance.HoopInRight ? 1 : -1;
        rb.velocity = new Vector2(jumpForce.x * direction, jumpForce.y);
    }
}
