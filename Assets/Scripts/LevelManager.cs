using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private EndOfLevelMenuInstance endOfLevelMenu;
    [SerializeField] private PlayerControllerInstance playerControllerInstance;
    [SerializeField] private GameObject startPoint;
    [SerializeField] private TimerInstance levelTimer;
    private Vector3 currentPoint;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {
        if (startPoint == null)
            Debug.LogError("No starting point are defined in level Manager.");
        else
            currentPoint = startPoint.transform.position;

        playerControllerInstance.Instance.canMove = false;
    }

    public void Save(Vector3 newPoint)
    {
        currentPoint = newPoint;
    }

    public Vector3 GetRespawnPoint()
    {
        return currentPoint;
    }

    public void StartPlay()
    {
        levelTimer.Instance.StartTimer();
        playerControllerInstance.Instance.canMove = true;
    }

    public void FinishPlay()
    {
        levelTimer.Instance.StopTimer();
        playerControllerInstance.Instance.canMove = false;
        endOfLevelMenu.Instance.gameObject.SetActive(true);
        endOfLevelMenu.Instance.RefreshTimer();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
