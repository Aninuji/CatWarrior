using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void Exit()
    {
        Application.Quit();
    }

    public static float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    /// <summary>
    /// Like <see cref="Mathf.Approximately(float, float)"/> but faster and you can actually put a threshold! Unity PLS
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}