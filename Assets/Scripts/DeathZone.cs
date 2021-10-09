using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public EventObject respawnEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.gameObject.transform.position = LevelManager.instance.getRespawnPoint();
            other.GetComponent<CharacterController>().enabled = true;
            respawnEvent.Trigger();
        }
    }
}
