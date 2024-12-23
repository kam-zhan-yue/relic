using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

public class GameOverPopup : Popup
{
    [SerializeField] private TextPopupItem restartButton;
    
    protected override void InitPopup()
    {
        Messenger.Default.Subscribe<ShowGameOverPayload>(OnGameOver);
        restartButton.onClick += Restart;
    }
    
    private void OnGameOver(ShowGameOverPayload payload)
    {
        ShowPopup();
    }

    private void Restart()
    {
        ServiceLocator.Instance.Get<IGameManager>().RestartGame();
    }
}