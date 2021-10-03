using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject[] waypoints;
    private int currentWayPoint = 0;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("toto");
        other.gameObject.transform.SetParent(gameObject.transform, true);
    }

    private void OnCollisionExit(Collision other)
    {
        other.gameObject.transform.parent = null;
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
