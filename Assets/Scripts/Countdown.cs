using UnityEngine;

public class Countdown : MonoBehaviour 
{
    public void CountdownEnded()
    {
        LevelManager.instance.StartPlay();
    }
}