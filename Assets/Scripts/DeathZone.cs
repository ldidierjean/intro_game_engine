using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(LevelManager.instance.getRespawnPoint());
            other.transform.position = LevelManager.instance.getRespawnPoint();
            Debug.Log(other.transform.position);
        }
    }
}
