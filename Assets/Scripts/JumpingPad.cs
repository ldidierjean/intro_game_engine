using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPad : MonoBehaviour
{
    public float bigJumpStrength = 18f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().Jump(bigJumpStrength);
        }
    }
}
