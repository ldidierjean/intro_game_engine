using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private GameObject startPoint;
    private GameObject currentPoint;

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
            currentPoint = startPoint;
    }

    public void save(GameObject newPoint)
    {
        if (newPoint.GetComponent<Checkpoint>() != null)
        {
            currentPoint = newPoint;
        }
    }
}
