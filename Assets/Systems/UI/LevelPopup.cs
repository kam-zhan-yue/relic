using DG.Tweening;
using Kuroneko.UIDelivery;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;

public class LevelPopup : Popup
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private TMP_Text title;
    private LevelSettings _data;
    
    protected override void InitPopup()
    {
        HidePopup();
        Messenger.Default.Subscribe<GamePayload>(InitGame);
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        if (_data == null) return;
        title.SetText($"{gameSettings.GetLevelNum(_data)} - {_data.name}");
    }

    private void InitGame(GamePayload payload)
    {
        _data = payload.GameState.CurrentLevel;
        if (_data == null) return;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(ShowPopup);
        sequence.AppendInterval(3f);
        sequence.AppendCallback(HidePopup);
    }
}
