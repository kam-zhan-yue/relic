using Kuroneko.AudioDelivery;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

public class StartPopup : Popup
{
    private bool _started = false;
    
    private GameState _gameState;
    
    protected override void InitPopup()
    {
        Messenger.Default.Subscribe<GamePayload>(InitGame);
    }

    private void InitGame(GamePayload payload)
    {
        _gameState = payload.GameState;
        _gameState.Cutscene();
    }
    
    private void Start()
    {
        // Prevents playing twice if somehow manages to load start scene again
        ServiceLocator.Instance.Get<IAudioService>().Stop("START");
        ServiceLocator.Instance.Get<IAudioService>().Play("START");
    }

    private void Update()
    {
        if (!_started && Input.GetMouseButtonDown(0))
        {
            _started = true;
            Messenger.Default.Publish(new StartGamePayload());
            HidePopup();
        }
    }
}
