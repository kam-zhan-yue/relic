using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamera : MonoBehaviour
{
    public Action OnEnter;
    public Action OnExit;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player _))
            OnEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player _))
            OnExit?.Invoke();
    }

    private void OnDestroy()
    {
        OnEnter = null;
        OnExit = null;
    }
}
