using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float colorDuration = 2.0f;

    [SerializeField] private Material inactivated;
    [SerializeField] private Material activated;
    Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    public void isTrigger()
    {
        LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lm.save(gameObject);

        _updateColor();
    }

    private void Update()
    {
        float lerp = Mathf.PingPong(Time.time, colorDuration) / colorDuration;
        renderer.material.Lerp(inactivated, activated, lerp);
    }

    private void _updateColor() // see https://forum.unity.com/threads/cant-get-material-lerp-to-work.8936/ for more precise one
    {
        for (float lerp = 0; lerp != 1; lerp++)
            renderer.material.Lerp(inactivated, activated, lerp);
}
}
