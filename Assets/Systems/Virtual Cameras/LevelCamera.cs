using Cinemachine;
using DG.Tweening;
using SuperMaxim.Messaging;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    [SerializeField] private bool doorAnimation = true;
    [SerializeField] private CinemachineVirtualCamera startCamera;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera doorCamera;
    [SerializeField] private CinemachineVirtualCamera zoomCamera;
    [SerializeField] private TriggerCamera trigger;
    private bool _completed = false;
    public bool CanPlayAnimation => doorAnimation && doorCamera;

    private void Awake()
    {
        playerCamera.Priority = 0;
        if (startCamera)
            startCamera.Priority = 10;
        if (doorCamera)
            doorCamera.Priority = 0;
        if (zoomCamera)
            zoomCamera.Priority = 0;
        Messenger.Default.Subscribe<GamePayload>(InitGame);
        Messenger.Default.Subscribe<LevelCompletePayload>(LevelComplete);
        trigger.OnEnter += Zoom;
        trigger.OnExit += UnZoom;
    }
    
    private void InitGame(GamePayload payload)
    {
        Transform playerTransform = payload.GameState.Player.transform;
        playerCamera.LookAt = playerTransform;
        playerCamera.Follow = playerTransform;
        zoomCamera.LookAt = playerTransform;
        zoomCamera.Follow = playerTransform;
    }

    public void Focus()
    {
        playerCamera.Priority = 15;
    }

    private void LevelComplete(LevelCompletePayload payload)
    {
        _completed = true;
        if (!CanPlayAnimation && zoomCamera)
            Zoom();
    }

    public void Door()
    {
        doorCamera.Priority = 20;
    }

    public void Player()
    {
        playerCamera.Priority = 25;
    }

    private void Zoom()
    {
        if (_completed)
            zoomCamera.Priority = 100;
    }

    private void UnZoom()
    {
        if (_completed)
            zoomCamera.Priority = 0;
    }
    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<GamePayload>(InitGame);
        Messenger.Default.Unsubscribe<LevelCompletePayload>(LevelComplete);
    }
}
