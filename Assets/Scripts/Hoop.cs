using System.Collections;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    [SerializeField] private HoopSetting setting;
    [SerializeField] private bool isRight;
    [SerializeField] private Collider2D[] colliders;
    private BoxCollider2D trigger;
    [SerializeField] private Animator netAnimator;
    [SerializeField] private HoopEffect effectManager;
    private Vector2 initPosition;

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
        Vector3 endPosition = initPosition + Vector2.right * setting.moveYAmount * (isRight ? -1 : 1);
        while (lerp < setting.getInTime)
        {
            lerp += Time.deltaTime;
            transform.position = Vector2.Lerp(initPosition, endPosition, lerp / setting.getInTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isScored && other.transform.position.y > transform.position.y + trigger.offset.y)
        {
            Ball.Instance.CheckCombo();
            isScored = true;
            StartCoroutine(GetOut());
            SetAnimation();
        }
    }

    private void SetAnimation()
    {
        if (GameManager.Instance.IsOnBurn)
        {
            netAnimator.SetTrigger("On Burn");
        }
        else
        {
            netAnimator.SetTrigger("On Score");
        }
        effectManager.Trigger();
    }

    private IEnumerator GetOut()
    {
        yield return Yielders.Get(setting.getOutDelay / 3);
        SetCollidersActive(false);
        yield return Yielders.Get(setting.getOutDelay * 2 / 3);
        float lerp = 0.0f;
        Vector2 startPosition = transform.position;
        while (lerp < setting.getOutTime)
        {
            lerp += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, initPosition, lerp / setting.getOutTime);
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
}
