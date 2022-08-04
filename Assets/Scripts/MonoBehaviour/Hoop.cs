using System.Collections;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    [SerializeField] private HoopSetting setting;
    [SerializeField] private bool isRight;
    [SerializeField] private Collider2D[] colliders;
    private BoxCollider2D trigger;
    [SerializeField] private ParticleSystem burnEffect;
    [SerializeField] private AddScoreEffect addScoreEffect;
    [SerializeField] private Transform displayBigScorePosition;
    private Animator animator;

    private Vector3 initPosition;

    private bool isScored = true;

    private void Start()
    {
        trigger = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        initPosition = transform.position;

        GameManager.Instance.OnScore += TriggerBigScoreEffect;
    }

    public void WakeUp()
    {
        float newPositionY = Random.Range(setting.lowestXPosition, setting.highestXPosition);
        Vector3 position = transform.position;
        position.y = newPositionY;
        transform.position = position;
        animator.Play("Get In", 2);
    }

    public void ReadyToScore()
    {
        isScored = false;
    }

    public void OnBallEnter(Collider2D other)
    {
        if (!isScored && other.transform.position.y > transform.position.y + trigger.offset.y)
        {
            if (GameManager.Instance.IsOnBurn)
            {
                animator.Play("Burn", 0);
            }
            else
            {
                animator.Play("Score", 0);
            }

            if (Ball.Instance.CheckCombo())
            {
                isScored = true;
                // StartCoroutine(GetOut());
                animator.Play("Get Out", 2);
            }
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
            animator.Play("Perfect", 1);
            addScoreEffect.Trigger(displayBigScorePosition.position);
        }
    }
}
