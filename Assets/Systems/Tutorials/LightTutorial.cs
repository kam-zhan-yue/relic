using System;
using SuperMaxim.Messaging;
using UnityEngine;

public class LightTutorial : Tutorial
{
    [SerializeField] private GhostBlock ghostBlock;

    protected override void Awake()
    {
        base.Awake();
        Messenger.Default.Subscribe<InteractivePayload>(OnInteractive);
        ghostBlock.onShow += Complete;
    }

    private void OnInteractive(InteractivePayload payload)
    {
        Activate();
    }
}    

