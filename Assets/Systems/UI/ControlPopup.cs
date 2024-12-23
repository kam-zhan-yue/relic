using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

public class ControlPopup : Popup
{
    [SerializeField] private LevelSelectPopup levelSelectPopup;
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private PausePopup pausePopup;
    
    protected override void InitPopup()
    {
        Messenger.Default.Subscribe<PausePayload>(OnPaused);
        pausePopup.onResume += Resume;
        pausePopup.onRestart += Restart;
        pausePopup.onLevelSelect += ShowLevelSelect;
        pausePopup.onSettings += ShowSettingsPopup;
        levelSelectPopup.onBack += ShowPausePopup;
        settingsPopup.onBack += ShowPausePopup;
    }

    private void OnPaused(PausePayload payload)
    {
        if (payload.Paused)
        {
            ShowPopup();
            ShowPausePopup();
        }
        else
        {
            HidePopup();
        }
    }

    private void ShowLevelSelect()
    {
        pausePopup.HidePopup();   
        levelSelectPopup.ShowPopup();
    }
    
    private void ShowPausePopup()
    {
        settingsPopup.HidePopup();
        levelSelectPopup.HidePopup();
        pausePopup.ShowPopup();
    }

    private void ShowSettingsPopup()
    {
        pausePopup.HidePopup();
        settingsPopup.ShowPopup();
    }

    private void Resume()
    {
        ServiceLocator.Instance.Get<IGameManager>().TogglePause();
    }

    private void Restart()
    {
        ServiceLocator.Instance.Get<IGameManager>().RestartGame();
    }
}