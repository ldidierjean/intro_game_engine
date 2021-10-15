using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventReceiver : MonoBehaviour
{
    public EventObject eventObject;
    public UnityEvent actions;

    private void OnEnable()
    {
        eventObject.Bind(actions.Invoke);
    }

    private void OnDisable()
    {
        eventObject.Unbind(actions.Invoke);
    }
}
