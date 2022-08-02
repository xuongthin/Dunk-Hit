using System.Collections;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    [SerializeField] private HoopSetting setting;
    [SerializeField] private bool isRight;
    [SerializeField] private Collider2D[] colliders;
    private BoxCollider2D trigger;
    [SerializeField] private Animator netAnimator;
    [SerializeField] private SpriteRenderer netSprite;
    [SerializeField] private SpriteRenderer burnSprite;
    [SerializeField] private ParticleSystem burnEffect;
    [SerializeField] private SpriteRenderer bigScoreSprite;
    private Animator animator;

    private Vector3 initPosition;

    private bool isScored = true;  // for safe, in case the ball hit trigger multiple times (cuz bouncing).

    private void Start()
    {
        trigger = GetComponent<BoxCollider2D>();
        initPosition = transform.position;
    }

    public void WakeUp()
    {
        initPosition.y = Random.Range(setting.lowestXPosition, setting.highestXPosition);
        SetCollidersActive(true);
        isScored = false;
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isScored && other.transform.position.y > transform.position.y + trigger.offset.y)
        {
            if (GameManager.Instance.IsOnBurn)
            {
                animator.SetTrigger("Burn Score");
            }
            else
            {
                netAnimator.SetTrigger("On Score");
            }
            if (Ball.Instance.CheckCombo())
            {
                TriggerBigScoreEffect();
            }
            isScored = true;
            StartCoroutine(GetOut());
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

        ResetDisplay();
    }

    private void SetCollidersActive(bool value)
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = value;
        }
    }

    private void TriggerNormalScoreEffect()
    {
    }

    public void PlayParticleEffect()
    {
        burnEffect.Play();
    }

    private void TriggerBigScoreEffect()
    {
        StartCoroutine(ScoreEffect());
    }

    private IEnumerator ScoreEffect()
    {
        int combo = GameManager.Instance.ComboCount;
        if (combo == 1)
            bigScoreSprite.sprite = setting.combo2;
        else if (combo == 2)
            bigScoreSprite.sprite = setting.combo4;
        else
            bigScoreSprite.sprite = setting.combo8[Random.Range(0, setting.combo8.Length)];

        bigScoreSprite.enabled = true;
        yield return Yielders.Get(setting.displayTime);
        float lerp = setting.fadeTime;
        Color temp = Color.white;
        while (lerp >= 0)
        {
            lerp -= Time.deltaTime;
            temp.a = lerp / setting.fadeTime;
            bigScoreSprite.color = temp;
        }

        bigScoreSprite.enabled = false;
        bigScoreSprite.color = Color.white;
    }

    private void ResetDisplay()
    {
        netSprite.enabled = true;
        burnSprite.enabled = false;
    }
}
