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
    [SerializeField] private Vector3 showScorePosition;
    private Animator animator;

    private Vector3 initPosition;

    private bool isScored = true;  // for safe, in case the ball hit trigger multiple times (cuz bouncing).

    private void Start()
    {
        trigger = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        initPosition = transform.position;
        showScorePosition.x *= isRight ? -1 : 1;

        GameManager.Instance.OnScore += TriggerBigScoreEffect;
    }

    public void WakeUp()
    {
        initPosition.y = Random.Range(setting.lowestXPosition, setting.highestXPosition);
        SetCollidersActive(true);
        StartCoroutine(GetIn());
    }

    private IEnumerator GetIn()
    {
        yield return Yielders.Get(setting.getInDelay);
        float lerp = 0.0f;
        Vector3 endPosition = initPosition + Vector3.right * setting.moveYAmount * (isRight ? -1 : 1);
        while (lerp < setting.getInTime)
        {
            lerp += Time.deltaTime;
            transform.position = Vector3.Lerp(initPosition, endPosition, lerp / setting.getInTime);
            yield return null;
        }
        isScored = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
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
                StartCoroutine(GetOut());
            }
        }
    }

    private IEnumerator GetOut()
    {
        yield return Yielders.Get(setting.getOutDelay / 3);
        SetCollidersActive(false);
        yield return Yielders.Get(setting.getOutDelay * 2 / 3);
        float lerp = 0.0f;
        Vector3 startPosition = transform.position;
        while (lerp < setting.getOutTime)
        {
            lerp += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, initPosition, lerp / setting.getOutTime);
            yield return null;
        }
    }

    private void SetCollidersActive(bool value)
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = value;
        }
    }

    public void PlayParticleEffect()
    {
        burnEffect.Play();
    }

    private void TriggerBigScoreEffect(bool combo)
    {
        if (combo && !isScored)
        {
            animator.Play("Perfect", 1);
            addScoreEffect.Trigger(transform.position + showScorePosition);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + showScorePosition, 0.1f);
    }
#endif
}
