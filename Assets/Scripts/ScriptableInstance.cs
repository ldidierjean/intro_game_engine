using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableInstance<T> : ScriptableObject where T: MonoBehaviour
{
    public T Instance { get; private set; }

    public bool RegisterInstance(T register)
    {
        if (register != null)
        {
            Instance = register;
            return true;
        }
        
        return false;
    }

    public bool UnregisterInstance(T unregister)
    {
        if (Instance == unregister)
        {
            Instance = null;
            return true;
        }

        return false;
    }
}
