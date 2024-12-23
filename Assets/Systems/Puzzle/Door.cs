using System;
using System.Collections;
using System.Collections.Generic;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

[Serializable]
public enum DoorType
{
    Entrance,
    Exit,
}

public class Door : MonoBehaviour, IOpenable
{
    private const float UNLOCK_PERCENTAGE = 0.4f;
    [SerializeField] private DoorType type = DoorType.Entrance;
    [SerializeField] private bool hasConnections = true;
    [SerializeField] private Transform leftAnchor;
    [SerializeField] private Transform rightAnchor;
    [SerializeField] private float openRotation = 70f;
    [SerializeField] private float openTime = 3f;
    [SerializeField] private bool reverse = false;
    [SerializeField] private bool playNormalSound = true;
    private List<Switch> connections = new List<Switch>();

    public DoorType Type => type;

    private float _openTimer;
    private bool Unlockable => _state != DoorState.Unlocked && type == DoorType.Exit;
    public bool Unlocked => _state == DoorState.Unlocked;

    private enum DoorState
    {
        Closed,
        Opened,
        Unlocked,
    }

    private DoorState _state = DoorState.Closed;

    private void Awake()
    {
        _openTimer = openTime;
    }

    private void Update()
    {
        if (hasConnections && _state != DoorState.Unlocked)
        {
            DoorState tempState = DoorState.Opened;
            foreach (Switch connection in connections)
            {
                if (!connection.activated)
                {
                    tempState = DoorState.Closed;
                    break;
                }
            }
            
            // only plays the gate sfx if there is a change of state
            if (_state == DoorState.Closed && tempState == DoorState.Opened)
            {
                // start gate and exit gate in the base environment get to play a louder sound -> not normal
                if (!playNormalSound)
                {
                    ServiceLocator.Instance.Get<IAudioService>().Stop("GATE_CLOSE");
                    ServiceLocator.Instance.Get<IAudioService>().Play("GATE_OPEN");
                }
                ServiceLocator.Instance.Get<IAudioService>().Stop("SOFT_CLOSE");
                ServiceLocator.Instance.Get<IAudioService>().Play("SOFT_OPEN");
                
            }
            else if (_state == DoorState.Opened && tempState == DoorState.Closed)
            {
                // start gate and exit gate in the base environment get to play a louder sound -> not normal
                if (!playNormalSound)
                {
                    ServiceLocator.Instance.Get<IAudioService>().Stop("GATE_OPEN");
                    ServiceLocator.Instance.Get<IAudioService>().Play("GATE_CLOSE");
                }
                
                ServiceLocator.Instance.Get<IAudioService>().Stop("SOFT_OPEN");
                ServiceLocator.Instance.Get<IAudioService>().Play("SOFT_CLOSE");
                
            }
            _state = tempState;
        }
        
        if (((!reverse && _state == DoorState.Opened) || _state == DoorState.Unlocked || (reverse && _state == DoorState.Closed)) && _openTimer > 0f)
        {
            _openTimer -= Time.unscaledDeltaTime;
            float openPercentage = 1f - _openTimer / openTime;
            leftAnchor.transform.localRotation = Quaternion.Euler(0f, -openPercentage * openRotation, 0f);
            rightAnchor.transform.localRotation = Quaternion.Euler(0f, openPercentage * openRotation, 0f);
            // Set to unlocked after a specific percentage
            if (Unlockable && openPercentage >= UNLOCK_PERCENTAGE)
            {
                UnlockState();
            }
        }
        else if ((_state == DoorState.Closed || (reverse && _state == DoorState.Opened)) && _openTimer < openTime)
        {
            _openTimer += Time.unscaledDeltaTime;
            float openPercentage = 1f - _openTimer / openTime;
            leftAnchor.transform.localRotation = Quaternion.Euler(0f, -openPercentage * openRotation, 0f);
            rightAnchor.transform.localRotation = Quaternion.Euler(0f, openPercentage * openRotation, 0f);
        }
    }

    public void Unlock()
    {
        if (!Unlockable) return;
        _openTimer = openTime;
        UnlockState();
    }

    private void UnlockState()
    {
        Messenger.Default.Publish(new LevelCompletePayload());
        _state = DoorState.Unlocked;
    }

    public void Open()
    {
        if (_state != DoorState.Unlocked)
        {
            _state = DoorState.Opened;
            _openTimer = openTime;
            ServiceLocator.Instance.Get<IAudioService>().Play("GATE_OPEN");
        }
    }

    public void Close()
    {
        if (_state != DoorState.Unlocked)
        {
            _state = DoorState.Closed;
            ServiceLocator.Instance.Get<IAudioService>().Play("GATE_CLOSE");
        }
    }

    public void AddConnection(Switch connection)
    {
        connections.Add(connection);
    }
}


