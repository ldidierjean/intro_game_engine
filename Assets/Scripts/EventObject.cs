using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Event", menuName = "Scriptable Objects/EventObject", order = 1)]
public class EventObject : ScriptableObject
{
    private UnityEvent respawnEvent;

    public void Bind(UnityAction callBack)
    {
        respawnEvent.AddListener(callBack);
    }

    public void Unbind(UnityAction callBack)
    {
        respawnEvent.RemoveListener(callBack);
    }

    public void Trigger()
    {
        respawnEvent.Invoke();
    }
}
