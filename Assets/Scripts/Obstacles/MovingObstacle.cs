using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private bool sticky;
    [SerializeField] private float speed;
    [SerializeField] private GameObject[] waypoints;
    private int currentWayPoint = 0;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sticky && other.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (sticky && other.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(null);
        }
    }

    private void Move()
    {
        if (transform.position == waypoints[currentWayPoint].transform.position)
        {
            currentWayPoint = (currentWayPoint + 1 >= waypoints.Length ?  0 : currentWayPoint + 1);
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWayPoint].transform.position, speed * Time.deltaTime);
    }
}
