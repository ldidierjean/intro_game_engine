using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float colorDuration = 2.0f;
    private bool isActive = false;

    [SerializeField] private Material inactivated;
    [SerializeField] private Material activated;

    private float triggerAnimationTime;

    private void Start()
    {
        if (!GetComponent<Renderer>())
            Debug.LogError("No renderer attach to the checkpoint");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            lm.save(gameObject.transform.GetChild(0).gameObject.transform.position);
            isActive = true;
            triggerAnimationTime = Time.time;
        }
    }

    private void Update()
    {
        if (isActive && GetComponent<Renderer>().material.color != activated.color)
        {
            float lerp = (Time.time - triggerAnimationTime) / colorDuration;
            GetComponent<Renderer>().material.Lerp(inactivated, activated, lerp);
        }
    }
}
