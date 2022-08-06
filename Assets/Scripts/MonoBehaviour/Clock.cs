using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public static Clock Instance;
    private void Awake()
    {
        Instance = this;
    }

    private float time;

    void Update()
    {
        time += Time.deltaTime;
    }

    public void Reset()
    {
        time = 0;
    }

    public void AddAlarm()
    {

    }
}
