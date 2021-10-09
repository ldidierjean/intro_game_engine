using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingObstacle : MonoBehaviour
{
    public EventObject respawnEvent;

    [SerializeField] float timeOut;
    [SerializeField] float yDisapear;

    private Vector3 initialPosition;
    private bool isFalling = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        if (!gameObject.GetComponent<Rigidbody>())
            Debug.LogError("Error: no rigibody is associated with the falling obstacle.");
        initialPosition = gameObject.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFalling && other.tag == "Player")
        {
            isFalling = true;
            StartCoroutine("falling");
        }
    }

    private void OnEnable()
    {
        respawnEvent.Bind(Respawn);
    }

    private void OnDisable()
    {
        respawnEvent.Unbind(Respawn);
    }

    private void Respawn()
    {
        isFalling = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        gameObject.transform.position = initialPosition;
        gameObject.SetActive(true);
    }

    IEnumerator falling()
    {
        yield return new WaitForSeconds(timeOut);
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        if (gameObject.transform.position.y < yDisapear)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.SetActive(false);
        }
        else
            yield return new WaitForSeconds(1);
    }
}
