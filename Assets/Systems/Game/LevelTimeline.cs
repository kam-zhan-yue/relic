using DG.Tweening;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

public class LevelTimeline : MonoBehaviour
{
    [SerializeField] private LevelCamera levelCamera;
    [SerializeField] private Transform center;
    [SerializeField] private StartDoor startDoor;
    private GameState _gameState;

    private void Awake()
    {
        Messenger.Default.Subscribe<GamePayload>(InitGame);
        Messenger.Default.Subscribe<LevelCompletePayload>(OnLevelComplete);
    }

    private void InitGame(GamePayload payload)
    {
        _gameState = payload.GameState;
        _gameState.Player.gameObject.SetActiveFast(false);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(startDoor.Door.Open);
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() => _gameState.Player.gameObject.SetActiveFast(true));
        sequence.Append(_gameState.Player.transform.DOMove(center.transform.position, 2f).SetEase(Ease.OutQuad)).SetUpdate(true);
        sequence.AppendCallback(startDoor.Door.Close);
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(startDoor.FadeOut);
        sequence.AppendCallback(() => levelCamera.Focus());
        sequence.AppendCallback(() => _gameState.Player.SetActive(true));
        sequence.AppendCallback(() => Messenger.Default.Publish(new InteractivePayload()));
    }
    
    private void OnLevelComplete(LevelCompletePayload payload)
    {
        if (levelCamera.CanPlayAnimation)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetUpdate(true);
            sequence.AppendCallback(() => _gameState.Player.SetActive(false));
            sequence.AppendCallback(() => Time.timeScale = 0f);
            sequence.AppendCallback(levelCamera.Door);
            sequence.AppendCallback(() => ServiceLocator.Instance.Get<IAudioService>().Play("LEVEL_COMPLETE"));
            sequence.AppendCallback(() => ServiceLocator.Instance.Get<IAudioService>().Pause("START"));
            // play level complete sfx when camera goes to the door
            sequence.AppendInterval(3f);
            sequence.AppendCallback(levelCamera.Player);
            sequence.AppendCallback(() => Time.timeScale = 1f);
            sequence.AppendCallback(() => _gameState.Player.SetActive(true));
            sequence.AppendCallback(() => ServiceLocator.Instance.Get<IAudioService>().Resume("START"));
        }
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<GamePayload>(InitGame);
        Messenger.Default.Unsubscribe<LevelCompletePayload>(OnLevelComplete);
    }
}
