using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name);
            other.transform.position = new Vector3(10, 10, 10);
/*            other.gameObject.transform.position = LevelManager.instance.getRespawnPoint();
            Debug.Log(other.transform.position);
*/        }
    }
}
