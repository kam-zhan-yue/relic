using DG.Tweening;
using Kuroneko.UIDelivery;
using TMPro;
using UnityEngine;

public class TutorialPopup : Popup
{
    [SerializeField] private Color completedColour;
    [SerializeField] private TMP_Text text;

    protected override void InitPopup()
    {
        HidePopup();
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        text.DOFade(1f, 2f).SetUpdate(true).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void Complete()
    {
        text.color = completedColour;
        text.DOFade(0f, 2f).SetUpdate(true).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }
}
