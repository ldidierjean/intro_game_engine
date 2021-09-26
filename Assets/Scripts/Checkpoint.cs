using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void isTrigger()
    {
        LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lm.save(gameObject);
    }
}
