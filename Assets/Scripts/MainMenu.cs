using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject menuCanvas;
    private GameObject settingsCanvas;
    private GameObject creditCanvas;

    void Start()
    {
        menuCanvas = GameObject.Find("MenuCanvas");
        settingsCanvas = GameObject.Find("SettingsCanvas");
        creditCanvas = GameObject.Find("CreditsCanvas");

        creditCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    public void LaunchGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void SeeSettings()
    {
        menuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void SeeCredits()
    {
        menuCanvas.SetActive(false);
        creditCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("exit");
        Application.Quit();
    }

    public void GoBack()
    {
        creditCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }
}
