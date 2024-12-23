using Cinemachine;
using DG.Tweening;
using SuperMaxim.Messaging;
using UnityEngine;

public class TrailerTimeline : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera startCamera;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    
    [SerializeField] private CameraSystem cameraSystem;
    [SerializeField] private Transform center;
    [SerializeField] private Door startDoor;
    
    private Player _player;

    private void Awake()
    {
        Messenger.Default.Subscribe<GamePayload>(InitGame);
    }

    private void InitGame(GamePayload payload)
    {
        _player = payload.GameState.Player;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => cameraSystem.StopShaking());
        sequence.AppendCallback(() => startDoor.Open());
        sequence.AppendInterval(1.5f);
        sequence.Append(_player.transform.DOMove(center.transform.position, 2f).SetEase(Ease.OutQuad));
        sequence.AppendCallback(() => startDoor.Close());
        sequence.AppendInterval(1f);
        sequence.AppendCallback(cameraSystem.StartDolly);
        sequence.AppendInterval(2f);
        sequence.AppendCallback(() => _player.SetActive(true));
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<GamePayload>(InitGame);
    }
}
