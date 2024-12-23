using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ClosingTimeline : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float firstTurnOn;
    [SerializeField] private float firstTurnOff;
    [SerializeField] private float secondTurnOn;
    [SerializeField] private float secondAngle;

    private void Start()
    {
        AnimationAsync().Forget();
    }

    private async UniTask AnimationAsync()
    {
        await UniTask.WaitForSeconds(firstTurnOn);
        player.SetFlashlightActive(true);
        await UniTask.WaitForSeconds(firstTurnOff);
        player.SetFlashlightActive(false);
        await UniTask.WaitForSeconds(secondTurnOn);
        player.SetFlashlightActive(true);
        player.SetFlashlightAngle(secondAngle);
    }
}
