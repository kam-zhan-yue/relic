using DG.Tweening;
using SuperMaxim.Messaging;
using UnityEngine;

public class StartTimeline : MonoBehaviour
{
    [SerializeField] private CameraSystem cameraSystem;
    [SerializeField] private Transform center;
    [SerializeField] private StartDoor startDoor;
    [SerializeField] private MovementTutorial movementTutorial;
    [SerializeField] private PauseTutorial pauseTutorial;
    
    private GameState _gameState;

    private void Awake()
    {
        Messenger.Default.Subscribe<GamePayload>(InitGame);
        Messenger.Default.Subscribe<StartGamePayload>(StartGame);

        movementTutorial.onComplete += () => pauseTutorial.Activate();
    }

    private void InitGame(GamePayload payload)
    {
        _gameState = payload.GameState;
    }

    private void StartGame(StartGamePayload _)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true);
        sequence.AppendCallback(_gameState.Running);
        sequence.AppendCallback(cameraSystem.StopShaking);
        sequence.AppendCallback(startDoor.Door.Open);
        sequence.AppendInterval(1.5f);
        sequence.Append(_gameState.Player.transform.DOMove(center.transform.position, 2f).SetEase(Ease.OutQuad).SetUpdate(true));
        sequence.AppendCallback(startDoor.Door.Close);
        sequence.AppendInterval(1f);
        sequence.AppendCallback(cameraSystem.StartDolly);
        sequence.AppendInterval(2f);
        sequence.AppendCallback(movementTutorial.Activate);
        sequence.AppendCallback(startDoor.FadeOut);
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() => _gameState.Player.SetActive(true));
    }
    
    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<GamePayload>(InitGame);
        Messenger.Default.Unsubscribe<StartGamePayload>(StartGame);
    }
}
