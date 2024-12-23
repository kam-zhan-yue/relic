using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectPopupItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color hoverColour;
    [SerializeField] private Color disabledColour;

    public Action<LevelSettings> onClick;
    private Color _originalColour;
    private TMP_Text _text;

    private LevelSettings _data;


    private bool _disabled;
    public bool Disabled
    {
        get => _disabled;
        set
        {
            _disabled = value;
            _text.color = _disabled ? disabledColour : _originalColour;
        }
    }

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _originalColour = _text.color;
    }

    public void Init(LevelSettings data, int level)
    {
        _data = data;
        _text.SetText($"{level} - {data.name}");
        Disabled = !data.IsUnlocked();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Disabled) return;
        onClick?.Invoke(_data);
        _text.color = _originalColour;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Disabled) return;
        _text.color = hoverColour;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Disabled) return;
        _text.color = _originalColour;
    }

    private void OnDestroy()
    {
        onClick = null;
    }
}
