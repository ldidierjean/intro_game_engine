using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelsCanvas;
    public GameObject menuCanvas;
    public GameObject settingsCanvas;
    public GameObject creditCanvas;

    void Start()
    {
        levelsCanvas.SetActive(false);
        creditCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    public void LaunchGame()
    {
        menuCanvas.SetActive(false);
        levelsCanvas.SetActive(true);
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
        levelsCanvas.SetActive(false);
        creditCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }
}
