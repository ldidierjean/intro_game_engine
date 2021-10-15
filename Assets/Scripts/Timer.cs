using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text text;

    private float currentTime = 0f;
    private bool isRunning = false;
    
    void Update()
    {
        if (isRunning)
            currentTime += Time.deltaTime;
        string a = Math.Round(currentTime, 2).ToString(CultureInfo.InvariantCulture);

        char[] separator = new char[1];
        separator[0] = '.';
        string[] t = a.Split(separator);
        if (t.Length == 1)
            text.text = t[0] + ":00";
        else
            text.text = t[0] + ":" + t[1] + (t[1].Length == 1 ? "0" : "");
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
