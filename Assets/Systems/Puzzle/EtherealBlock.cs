using System;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

public class EtherealBlock : EtherealBehaviour, ISwitchable
{
    [SerializeField, TableList] private EtherealPosition[] positions = Array.Empty<EtherealPosition>();

    private EtherealPosition _previousPosition = null; // Store the previous position for sfx
    private ParticleSystem ps;

    protected override void Start()
    {
        base.Start();

        foreach (var position in positions)
        {
            position.SetParentBlock(this);
        }
        
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
    }    

    protected override void Update()
    {
        base.Update();
        EtherealPosition showingBlock = null;
        
        foreach (var position in positions)
        {
            // Instantly switch to EtherealPosition here as it 
            // will handle its own hiding logic
            if (position.showing)
            {
                showingBlock = position;
            }
        }

        if (!showingBlock) return;
        
        // Check if the current showingBlock is different from the previous position
        if (showingBlock != _previousPosition)
        {
            // Play sound effect only if the position has changed
            ServiceLocator.Instance.Get<IAudioService>().Play("QUANTUM_TELEPORT");
            ps.Stop();
            ps.Play();
            _previousPosition = showingBlock; // Update the previous position
        }
        
        transform.position = showingBlock.transform.position;
        
        base.Update();
    }

    public override void Show()
    {
        base.Show();
        meshRenderer.enabled = true;
        transparent.gameObject.SetActiveFast(true);
    }

    public override void Hide()
    {
        base.Hide();
        meshRenderer.enabled = false;
        transparent.gameObject.SetActiveFast(false);
    }

    public override void DeleteShadow()
    {
        base.DeleteShadow();
        transform.position = new Vector3(0, -10, 0);
    }
}
