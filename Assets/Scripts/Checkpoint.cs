using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float colorDuration = 2.0f;
    private bool isActive = false;

    [SerializeField] private Material inactivated;
    [SerializeField] private Material activated;
    Renderer renderer;

    private float triggerAnimationTime;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    public void isTrigger()
    {
        LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lm.save(gameObject);
        isActive = true;
        triggerAnimationTime = Time.time;
    }

    private void Update()
    {
        if (isActive && renderer.material.color != activated.color)
        {
            float lerp = (Time.time - triggerAnimationTime) / colorDuration;
            renderer.material.Lerp(inactivated, activated, lerp);
        }
    }
}
