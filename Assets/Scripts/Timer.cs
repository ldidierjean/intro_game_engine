using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TimerInstance instance;
    public FloatInstance currentTimeHolder;
    public float CurrentTime => currentTime;

    private float currentTime = 0f;
    private bool isRunning = false;

    private void OnEnable()
    {
        instance.RegisterInstance(this);
    }

    private void OnDisable()
    {
        instance.UnregisterInstance(this);
    }

    void Update()
    {
        if (isRunning)
            currentTime += Time.deltaTime;
        currentTimeHolder.value = currentTime;
        string a = Math.Round(currentTime, 2).ToString(CultureInfo.InvariantCulture);

        char[] separator = new char[1];
        separator[0] = '.';
        string[] t = a.Split(separator);
        if (t.Length == 1)
            text.text = t[0] + ".00";
        else
            text.text = t[0] + "." + t[1] + (t[1].Length == 1 ? "0" : "");
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
