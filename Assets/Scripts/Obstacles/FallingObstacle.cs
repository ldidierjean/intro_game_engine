using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacle : MonoBehaviour
{
    [SerializeField] float timeOut;
    private bool isActivated = false;

    private void Start()
    {
        if (!gameObject.GetComponent<Rigidbody>())
            Debug.LogError("Error: no rigibody is associated with the falling obstacle.");
    }

    private void Update()
    {
        if (isActivated)
        {
            timeOut -= Time.deltaTime;
            if (timeOut < 0)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
            }

        }
        if (transform.position.y <= -10)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isActivated = true;
        }
    }
}
