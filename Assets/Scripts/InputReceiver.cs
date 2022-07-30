using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputReceiver : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent OnTouch;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch?.Invoke();
    }
}
