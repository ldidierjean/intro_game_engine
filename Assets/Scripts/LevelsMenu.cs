using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct LevelInfo
{
    public string name;
    public string identifier;
}

public class LevelsMenu : MonoBehaviour
{
    public GameObject buttonPrefab;
    public RectTransform buttonParent;
    public LevelInfo[] levels;

    void Start()
    {
        foreach (var level in levels)
        {
            GameObject button = Instantiate(buttonPrefab, buttonParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = level.name;
            button.GetComponent<Button>().onClick.AddListener((() => LoadLevel(level.identifier)));
        }
    }

    public void LoadLevel(string identifier)
    {
        SceneManager.LoadScene(identifier);
    }
}
