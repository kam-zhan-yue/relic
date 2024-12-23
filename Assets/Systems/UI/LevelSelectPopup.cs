using System;
using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class LevelSelectPopup : Popup
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private RectTransform holder;
    [SerializeField] private LevelSelectPopupItem samplePopupItem;
    [SerializeField] private TextPopupItem backButton;

    private readonly List<LevelSelectPopupItem> _popupItems = new();
    
    public Action onBack;
    
    protected override void InitPopup()
    {
        backButton.onClick += Back;
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        TryInstantiate();

        for (int i = 0; i < _popupItems.Count; ++i)
        {
            _popupItems[i].gameObject.SetActiveFast(false);
            _popupItems[i].onClick = null;
        }


        int level = 1;
        for (int i = 0; i < gameSettings.levels.Length; ++i)
        {
            if (i < _popupItems.Count && gameSettings.levels[i].active && gameSettings.levels[i].menu)
            {
                _popupItems[i].gameObject.SetActiveFast(true);
                _popupItems[i].Init(gameSettings.levels[i], level);
                _popupItems[i].onClick += LoadLevel;
                level += 1;
            }
        }
        
        backButton.transform.SetAsLastSibling();
    }

    private void TryInstantiate()
    {
        int numToSpawn = gameSettings.levels.Length - _popupItems.Count;
        if (numToSpawn > 0)
        {
            samplePopupItem.gameObject.SetActiveFast(true);
            for (int i = 0; i < numToSpawn; ++i)
            {
                LevelSelectPopupItem item = Instantiate(samplePopupItem, holder);
                _popupItems.Add(item);
            }
        }
        
        samplePopupItem.gameObject.SetActiveFast(false);
    }

    private void LoadLevel(LevelSettings data)
    {
        ServiceLocator.Instance.Get<IGameManager>().LoadLevel(data);
    }

    private void Back() => onBack?.Invoke();

    private void OnDestroy()
    {
        onBack = null;
    }
}
