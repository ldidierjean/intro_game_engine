using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timer = 0f;
    public Text text;

    void Update()
    {
        timer += Time.deltaTime;
        string a = System.Math.Round(timer, 2).ToString();

        char[] separator = new char[1];
        separator[0] = '.';
        string[] t = a.Split(separator);
        if (t.Length == 1)
            text.text = t[0] + ":00";
        else
            text.text = t[0] + ":" + t[1] + (t[1].Length == 1 ? "0" : "");
    }
}
