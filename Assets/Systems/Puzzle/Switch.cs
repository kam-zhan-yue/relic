using System;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Switch : MonoBehaviour
{
    [SerializeField] private bool manual;
    [ShowIf("manual"), SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField, TableList] private Door[] connections = Array.Empty<Door>();
    public bool activated;
    private static readonly int Pressed = Shader.PropertyToID("_Pressed");

    private void Awake()
    {
        if(!manual)
            meshRenderer = GetComponent<MeshRenderer>();

        foreach (var connection in connections)
        {
            connection.AddConnection(this);
        }
    }

    protected void Activate()
    {
        if (activated) return;
        activated = true;
        // meshRenderer.material = onMaterial;
        meshRenderer.material.SetFloat(Pressed, 1);
        ServiceLocator.Instance.Get<IAudioService>().Play("SWITCH");
    }

    protected void Deactivate()
    {
        if (!activated) return;
        activated = false;
        // meshRenderer.material = offMaterial;
        meshRenderer.material.SetFloat(Pressed, 0);
    }
}