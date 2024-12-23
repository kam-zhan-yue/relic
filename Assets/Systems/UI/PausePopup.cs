using System;
using Kuroneko.UIDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

public class PausePopup : Popup
{
    [SerializeField] private TextPopupItem resumeButton;
    [SerializeField] private TextPopupItem restartButton;
    [SerializeField] private TextPopupItem settingsButton;
    [SerializeField] private TextPopupItem levelSelectButton;

    public Action onResume;
    public Action onRestart;
    public Action onSettings;
    public Action onLevelSelect;
    
    protected override void InitPopup()
    {
        Messenger.Default.Subscribe<PausePayload>(OnPaused);
        resumeButton.onClick += () => onResume?.Invoke();
        restartButton.onClick += () => onRestart?.Invoke();
        settingsButton.onClick += () => onSettings?.Invoke();
        levelSelectButton.onClick += () => onLevelSelect?.Invoke();
    }
    
    private void OnPaused(PausePayload payload)
    {
        if (payload.Paused)
            ShowPopup();
        else
            HidePopup();
    }
    
    private void OnDestroy()
    {
        onResume = null;
        onRestart = null;
        onLevelSelect = null;
    }
}