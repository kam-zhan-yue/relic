using System;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextPopupItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color hoverColour;

    public Action onClick;
    private Color _originalColour;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _originalColour = _text.color;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        onClick?.Invoke();
        _text.color = _originalColour;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ServiceLocator.Instance.Get<IAudioService>().Play("Sw");
        _text.color = hoverColour;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.color = _originalColour;
    }

    private void OnDestroy()
    {
        onClick = null;
    }
}
