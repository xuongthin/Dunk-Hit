using System.Collections;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    [SerializeField] private HoopSetting setting;
    [SerializeField] private bool isRight;
    [SerializeField] private Collider2D[] colliders;
    [SerializeField] private Transform trigger;
    [SerializeField] private ParticleSystem burnEffect;
    [SerializeField] private AddScoreEffect addScoreEffect;
    [SerializeField] private Transform displayBigScorePosition;
    private Animator animator;

    private bool isScored = true;

    private const string GET_IN = "Get In";
    private const string GET_OUT = "Get Out";
    private const string BURN = "Burn";
    private const string SCORE = "Score";
    private const string PERFECT = "Perfect";

    private void Start()
    {
        animator = GetComponent<Animator>();

        GameManager.Instance.OnScore += TriggerBigScoreEffect;
    }

    public void WakeUp()
    {
        float newPositionY = Random.Range(setting.lowestXPosition, setting.highestXPosition);
        Vector3 position = transform.position;
        position.y = newPositionY;
        transform.position = position;
        animator.Play(GET_IN, 0);
    }

    public void ReadyToScore()
    {
        isScored = false;
    }

    public void OnBallExit(Collider2D other)
    {
        if (!isScored && other.transform.position.y < trigger.position.y)
        {
            if (GameManager.Instance.IsOnBurn)
            {
                animator.Play(BURN, 1);
            }
            else
            {
                animator.Play(SCORE, 1);
            }

            Ball.Instance.CheckCombo();

            isScored = true;
            animator.Play(GET_OUT, 0);
        }
    }

    public void EnableColliders()
    {
        SetCollidersActive(true);
    }

    public void DisableColliders()
    {
        SetCollidersActive(false);
    }

    public void PlayParticleEffect()
    {
        burnEffect.Play();
    }

    private void SetCollidersActive(bool value)
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = value;
        }
    }

    private void TriggerBigScoreEffect(bool combo)
    {
        if (combo && !isScored)
        {
            animator.Play(PERFECT, 2);
            addScoreEffect.Trigger(displayBigScorePosition.position);
        }
    }
}
