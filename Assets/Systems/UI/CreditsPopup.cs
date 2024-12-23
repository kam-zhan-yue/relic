using Kuroneko.UIDelivery;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsPopup : Popup
{
    [SerializeField] private TextPopupItem restartButton;
    
    protected override void InitPopup()
    {
        restartButton.onClick += () => SceneManager.LoadScene(0);
    }
}