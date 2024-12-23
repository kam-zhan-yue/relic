using System;
using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

public class PressurePlate : Switch
{
    private void Update() {
        // Get all colliders within range of the pressure plate
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity);
        // Filter by ISwitchable interface
        List<Collider> weights = new List<Collider>();
        for (int i=0;i<hitColliders.Length;i++) {
            if (hitColliders[i].TryGetComponent(out ISwitchable _)) {
                weights.Add(hitColliders[i]);
            }
        }
        // Check if weight is bigger than 1.0
        if (weights.Count > 0) {
            Activate();
        } else {
            Deactivate();
        }
    }
}
