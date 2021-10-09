using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacle : MonoBehaviour
{
    [SerializeField] float timeOut;
    [SerializeField] float yDestruction;

    private void Start()
    {
        if (!gameObject.GetComponent<Rigidbody>())
            Debug.LogError("Error: no rigibody is associated with the falling obstacle.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine("falling");
        }
    }

    IEnumerator falling()
    {
        Debug.Log("start coroutine");
        yield return new WaitForSeconds(timeOut);
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        if (gameObject.transform.position.y <= yDestruction)
        {
            Destroy(gameObject);
        }
        else
            yield return new WaitForSeconds(1);
    }
}
