using DG.Tweening;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

public enum FlowState
{
    Running = 0,
    Paused = 1,
    GameOver = 2,
    Restart = 3,
    Completed = 4,
    Cutscene = 5,
}

public class GameState
{
    private readonly GameSettings _settings;
    private Player _player;
    private int _activeSwitches;

    public Player Player => _player;
    public LevelSettings CurrentLevel => _settings.GetCurrentLevel();
    public FlowState State { get; private set; } = FlowState.Running;

    private bool CanTogglePause => State != FlowState.Cutscene && State != FlowState.Restart;
    
    public GameState(GameSettings settings)
    {
        _settings = settings;
    }

    public void Init()
    {
        Time.timeScale = 1f;
        _player = Object.FindObjectOfType<Player>();
    }

    public void LevelCompleted(LevelCompletePayload _)
    {
        State = FlowState.Completed;
        _player.SetFlashlightActive(false);
        CurrentLevel.Unlock();
    }

    public void GameOver(GameOverPayload payload)
    {
        if (State == FlowState.GameOver)
            return;
        State = FlowState.GameOver;
        // Turn off the flashlight and stop the player's movement
        _player.SetActive(false);
        _player.SetFlashlightActive(false);
        _player.SetAnimation(false);
        Cutscene();
        ServiceLocator.Instance.Get<IAudioService>().Pause("START");
        Sequence sequence = DOTween.Sequence();
        // Rotate the player to the direction of the statue
        Vector3 statuePosition = payload.Statue.transform.position;
        Vector3 lookAt = new Vector3(statuePosition.x, _player.transform.position.y, statuePosition.z);
        sequence.Append(_player.transform.DOLookAt(lookAt, 1f));
        // Move the camera to the player's perspective
        sequence.AppendCallback(() => _player.SetPriority(100));
        sequence.AppendInterval(2f);
        // Reveal the statue
        sequence.AppendCallback(() => _player.SetFlashlightActive(true));
        sequence.AppendCallback(() => _player.SetFlashlightAngle(120f));
        sequence.AppendCallback(() =>
            ServiceLocator.Instance.Get<IAudioService>().Play("STATUE_APPEAR"));
        // Show the UI
        sequence.AppendInterval(1f);
        sequence.AppendCallback(ShowGameOver);
        sequence.AppendCallback(() => State = FlowState.Restart);
    }

    private void ShowGameOver()
    {
        Time.timeScale = 0f;
        Messenger.Default.Publish(new ShowGameOverPayload());
    }

    public void TogglePause()
    {
        if (!CanTogglePause)
            return;
        if (State != FlowState.Paused)
            Pause();
        else if (State == FlowState.Paused)
            Resume();
        
        PausePayload payload = new PausePayload();
        payload.Paused = State == FlowState.Paused;
        Messenger.Default.Publish(payload);
    }

    private void Pause()
    {
        // ServiceLocator.Instance.Get<IAudioService>().Pause("START");
        State = FlowState.Paused;
        _player.SetActive(false);
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        // ServiceLocator.Instance.Get<IAudioService>().Resume("START");
        State = FlowState.Running;
        _player.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Cutscene()
    {
        State = FlowState.Cutscene;
    }

    public void Running()
    {
        State = FlowState.Running;
    }
}