using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventComponent : MonoBehaviour
{
    public UnityEvent actions;

    public void Trigger()
    {
        actions.Invoke();
    }
}
