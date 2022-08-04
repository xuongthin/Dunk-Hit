using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCallBack : MonoBehaviour
{
    public Collider2DEvent OnTriggerEnter;

    private void OnTriggerExit2D(Collider2D other)
    {
        OnTriggerEnter?.Invoke(other);
    }
}

[System.Serializable]
public class Collider2DEvent : UnityEvent<Collider2D>
{

}