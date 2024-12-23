using System.Text;
using Cysharp.Threading.Tasks;
using Kuroneko.UIDelivery;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PrototypePopup : Popup
{
    [SerializeField] private TMP_Text text;
    private LevelSettings _data;
    
    protected override void InitPopup()
    {
        HidePopup();
    }
}
