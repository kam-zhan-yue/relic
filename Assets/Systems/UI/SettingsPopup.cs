using System;
using Kuroneko.UIDelivery;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : Popup
{
    [SerializeField] private AudioSettings audioSettings;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextPopupItem backButton;

    public Action onBack;
    
    protected override void InitPopup()
    {
        backButton.onClick += () => onBack?.Invoke();
        bgmSlider.onValueChanged.AddListener(audioSettings.OnBgmChanged);
        sfxSlider.onValueChanged.AddListener(audioSettings.OnSfxChanged);
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        bgmSlider.value = audioSettings.Bgm;
        sfxSlider.value = audioSettings.Sfx;
    }

    private void OnDestroy()
    {
        onBack = null;
        bgmSlider.onValueChanged.RemoveListener(audioSettings.OnBgmChanged);
        sfxSlider.onValueChanged.RemoveListener(audioSettings.OnSfxChanged);
    }
}