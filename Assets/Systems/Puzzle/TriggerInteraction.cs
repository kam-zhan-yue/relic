using System;
using System.Collections;
using System.Collections.Generic;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class TriggerInteraction : MonoBehaviour
{
    [SerializeField] private Door door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player _) && !door.Unlocked)
        {
            ServiceLocator.Instance.Get<IAudioService>().Play("GATE_OPEN");
            door.Unlock();
        }
    }
}
