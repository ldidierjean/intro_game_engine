using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private GameObject startPoint;
    [SerializeField] private Timer levelTimer;
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
        levelTimer.StartTimer();
    }
}
