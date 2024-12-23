using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostWall : MonoBehaviour
{
    private EtherealBehaviour[] objectsWithShadows;
    public Action onDelete;
    
    private void Start()
    {
        objectsWithShadows = FindObjectsByType<EtherealBehaviour>(FindObjectsSortMode.None);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out Player _))
        {
            return;
        }

        onDelete?.Invoke();
        foreach (EtherealBehaviour shadow in objectsWithShadows)
        {
            shadow.DeleteShadow();
        }
    }

    private void OnDestroy()
    {
        onDelete = null;
    }
}