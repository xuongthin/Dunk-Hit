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
        animator.Play("Get In", 0);
    }

    public void ReadyToScore()
    {
        isScored = false;
    }

    public void OnBallEnter(Collider2D other)
    {
        if (!isScored && other.transform.position.y > trigger.position.y)
        {
            if (GameManager.Instance.IsOnBurn)
            {
                animator.Play("Burn", 1);
            }
            else
            {
                animator.Play("Score", 1);
            }

            if (Ball.Instance.CheckCombo())
            {
                isScored = true;
                animator.Play("Get Out", 0);
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
            animator.Play("Perfect", 2);
            addScoreEffect.Trigger(displayBigScorePosition.position);
        }
    }
}
