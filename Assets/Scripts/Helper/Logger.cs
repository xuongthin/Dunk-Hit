using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger
{
    public static void Log(string log)
    {
#if UNITY_EDITOR
        Debug.Log(log);
#endif
    }
}
