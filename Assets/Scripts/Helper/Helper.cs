using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    /// <summary>
    /// This function return a rect that clamped 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="bound"></param>
    /// <returns></returns>
    public static Rect ClampRect(Rect target, Rect bound)
    {
        Vector2 position = target.position;

        if (target.xMin < bound.xMin)
        {
            position.x += bound.xMin - target.xMin;
        }
        else if (target.xMax > bound.xMax)
        {
            position.x += bound.xMax - target.xMax;
        }

        if (target.yMin < bound.yMin)
        {
            position.y += bound.yMin - target.yMin;
        }
        else if (target.yMax > bound.yMax)
        {
            position.y += bound.yMax - target.yMax;
        }

        target.position = position;
        return target;
    }

    /// <summary>
    /// return a rect that is outside of bound (for now, it just works with square-rect)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="bound"></param>
    /// <returns></returns>
    public static Rect UnclampRect(Rect target, Rect bound)
    {
        if (!target.Overlaps(bound))
            return target;

        Vector2 position = target.position;
        Vector2 invadeDirection = bound.center - target.center;
        if (Mathf.Abs(invadeDirection.x) > Mathf.Abs(invadeDirection.y))
        {
            position.x += ((target.size.x / 2 + bound.size.x / 2) - Mathf.Abs(invadeDirection.x)) * Mathf.Sign(-invadeDirection.x);
        }
        else
        {
            position.y += ((target.size.y / 2 + bound.size.y / 2) - Mathf.Abs(invadeDirection.y)) * Mathf.Sign(-invadeDirection.y);
        }
        target.position = position;
        return target;
    }

    // public static bool CompareNSet<T>(this T current, T other, CompareType compareType) where T : IComparable
    // {
    //     switch (compareType)
    //     {
    //         case CompareType.Equal:
    //             current.ReplayOrNot<T>(other, currentCompareNSet )
    //             break;
    //         default:
    //             break;
    //     }
    //     return false;
    // }

    // public static bool ReplayOrNot<T>(this T current, T other, bool doit)
    // {
    //     if (doit)
    //         current = other;

    //     return doit;
    // }

    /// <summary>
    /// Logs a message to Unity Console (only work in Editor)
    /// </summary>
    /// <param name="log"></param>
    public static void Log(this string log)
    {
#if UNITY_EDITOR
        Debug.Log(log);
#endif
    }
}

// public enum CompareType
// {
//     Equal,
//     Greater,
//     Less,
//     GreaterOrEqual,
//     LessOrEqual,
//     Different
// }
