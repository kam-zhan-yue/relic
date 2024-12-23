using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class TransitionPopup : Popup
{
    [SerializeField] private Image panel;
    [SerializeField] private bool blackOnStart = false;
    
    protected override void InitPopup()
    {
        Messenger.Default.Subscribe<NextLevelPayload>(OnNextLevel);
    }

    private void Start()
    {
        if (blackOnStart)
        {
            Color colour = panel.color;
            colour.a = 1f;
            panel.color = colour;
            HidePopup();
        }
    }

    private void OnNextLevel(NextLevelPayload payload)
    {
        ShowPopup();
    }

    protected override void OnDoneShowing()
    {
        ServiceLocator.Instance.Get<IGameManager>().NextLevel();
    }
}
