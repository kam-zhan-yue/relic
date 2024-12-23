using System.Collections;
using System.Collections.Generic;
using SuperMaxim.Messaging;
using UnityEngine;

public class GhostWallTutorial : Tutorial
{
    [SerializeField] private GhostWall wall1;
    [SerializeField] private GhostWall wall2;

    protected override void Awake()
    {
        base.Awake();
        Messenger.Default.Subscribe<InteractivePayload>(OnInteractive);
        wall1.onDelete += Complete;
        wall2.onDelete += Complete;
    }

    private void OnInteractive(InteractivePayload payload)
    {
        Activate();
    }
}
