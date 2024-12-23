using System;
using DG.Tweening;
using SuperMaxim.Messaging;
using UnityEngine;

public class StartDoor : MonoBehaviour
{
    private const float FADE_THRESHOLD = 4f;
    private const float SOLID_THRESHOLD = 2f;
    private const float TRANSPARENT = 0.05f;
    
    private MeshRenderer[] _meshRenderers = Array.Empty<MeshRenderer>();
    private Door _door;
    private bool _faded;
    public Door Door => _door;
    private Player _player;

    private void Awake()
    {
        Messenger.Default.Subscribe<GamePayload>(InitGame);
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _door = GetComponent<Door>();
    }

    private void InitGame(GamePayload payload)
    {
        _player = payload.GameState.Player;
    }

    public void FadeOut()
    {
        float x = 1f;
        DOTween.To(() => x, SetFade, TRANSPARENT, 1f)
            .OnComplete(() => _faded = true)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    private void Update()
    {
        if (_faded)
        {
            float distance = (_player.transform.position - transform.position).magnitude;
            float fade;
            if (distance <= SOLID_THRESHOLD)
                fade = 1f;
            else if (distance >= FADE_THRESHOLD)
                fade = TRANSPARENT;
            else
            {
                float normalizedDistance = (distance - SOLID_THRESHOLD) / (FADE_THRESHOLD - SOLID_THRESHOLD);
                fade = Mathf.Lerp(1f, TRANSPARENT, normalizedDistance);
            }

            SetFade(fade);
        }
    }

    private void SetFade(float alpha)
    {
        for (int i = 0; i < _meshRenderers.Length; ++i)
        {
            Color original = _meshRenderers[i].material.color;
            original.a = alpha;
            _meshRenderers[i].material.color = original;
        }
    }
}
