using System;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class GhostBlock : EtherealBehaviour, ISwitchable
{
    private Rigidbody _rb;
    public Action onShow;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player _))
            _rb.isKinematic = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player _))
            _rb.isKinematic = true;
    }

    public override void Show()
    {
        base.Show();
        onShow?.Invoke();
    }

    private void OnDestroy()
    {
        onShow = null;
    }
}