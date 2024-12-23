using System;
using DG.Tweening;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class EndTimeline : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float doorOpen = 3f;
    [SerializeField] private float flickerOne = 3f;
    [SerializeField] private float flickerTwo = 1f;
    [SerializeField] private float blackOut = 1f;
    [SerializeField] private float on = 3f;
    [SerializeField] private float finalLight = 1f;
    
    private Light[] _lights = Array.Empty<Light>();
    private Statue[] _statues = Array.Empty<Statue>();
    private float[] _intensities = Array.Empty<float>();

    private void Awake()
    {
        _lights = FindObjectsOfType<Light>();
        _statues = FindObjectsOfType<Statue>();
        _intensities = new float[_lights.Length];
        for (int i = 0; i < _lights.Length; ++i)
            _intensities[i] = _lights[i].intensity;
        for (int i = 0; i < _statues.Length; ++i)
            _statues[i].SetShowpiece();
    }
    
    private void Start()
    {
        ServiceLocator.Instance.Get<IAudioService>().Stop("START");
        player.SetAnimation(true);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(flickerOne);
        sequence.Append(Flicker());
        sequence.AppendCallback(() => player.SetFlashlightActive(true));
        sequence.AppendInterval(flickerTwo);
        sequence.Append(Flicker(0.5f));
        sequence.AppendInterval(blackOut);
        sequence.AppendCallback(() => player.SetFlashlightActive(false));
        sequence.Append(Flicker(0.1f));
        sequence.AppendInterval(on);
        sequence.AppendCallback(ShowStatues);
        sequence.Append(Flicker());
        sequence.AppendInterval(finalLight);
        sequence.AppendCallback(() => player.SetFlashlightActive(true));
        sequence.AppendCallback(() => player.SetFlashlightAngle(130f));
        sequence.AppendCallback(() =>
            ServiceLocator.Instance.Get<IAudioService>().Play("STATUE_APPEAR"));
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() => ServiceLocator.Instance.Get<IGameManager>().NextLevel());

        Sequence doorSequence = DOTween.Sequence();
        doorSequence.AppendInterval(doorOpen);
        doorSequence.AppendCallback(() => ServiceLocator.Instance.Get<IAudioService>().Play("GATE_OPEN"));
    }

    private Sequence Flicker(float magnitude = 1f)
    {
        Sequence flickerSequence = DOTween.Sequence();
        flickerSequence.AppendCallback(() =>
            ServiceLocator.Instance.Get<IAudioService>().Play("FLICKER"));

        for (int i=0; i<_lights.Length; ++i)
        {
            Light sceneLight = _lights[i];
            float originalIntensity = _intensities[i] * magnitude;
            Sequence lightSequence = DOTween.Sequence();
            lightSequence.Append(sceneLight.DOIntensity(0f, 0.1f)) // Turn off
                .Append(sceneLight.DOIntensity(originalIntensity, 0.1f)) // Turn back on
                .Append(sceneLight.DOIntensity(0f, 0.05f)) // Flicker off briefly
                .Append(sceneLight.DOIntensity(originalIntensity, 0.05f)); // Flicker back on
            flickerSequence.Join(lightSequence);
        }

        return flickerSequence;
    }

    private void ShowStatues()
    {
        for (int i = 0; i < _statues.Length; ++i)
        {
            _statues[i].Show();
        }
    }
}
