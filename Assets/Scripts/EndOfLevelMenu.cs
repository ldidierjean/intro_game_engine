using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelMenu : MonoBehaviour
{
    public EndOfLevelMenuInstance instance;
    public TimerInstance timer;
    public TextMeshProUGUI text;

    private void Awake()
    {
        instance.RegisterInstance(this);
    }

    private void OnDestroy()
    {
        instance.UnregisterInstance(this);
    }

    private void Start()
    {
        RefreshTimer();
        gameObject.SetActive(false);
    }

    public void RefreshTimer()
    {
        string a = Math.Round(timer.Instance.CurrentTime, 2).ToString(CultureInfo.InvariantCulture);

        char[] separator = new char[1];
        separator[0] = '.';
        string[] t = a.Split(separator);
        if (t.Length == 1)
            text.text = t[0] + ".00";
        else
            text.text = t[0] + "." + t[1] + (t[1].Length == 1 ? "0" : "");
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
