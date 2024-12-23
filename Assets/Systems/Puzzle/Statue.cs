using System;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

public class Statue : EtherealBehaviour, ISwitchable
{
    private const float THRESHOLD = 1.75f;
    private bool activated = false; // If player has moved torch over them for the first time
    private bool canMove = false; // If statue is currently in the torch

    [SerializeField] private float maxMoveSpeed = 2.0f;
    [SerializeField] private float decelerationTime = 0.2f;
    
    private float moveSpeed = 0.0f;
    private bool turning = false;
    private Player player;
    private Rigidbody _rigidbody;
    private float _timer = 0f;
    private GameState _state;
    private bool _showpiece = false;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
        Messenger.Default.Subscribe<GamePayload>(Init);
    }

    private void Init(GamePayload payload)
    {
        _state = payload.GameState;
        player = payload.GameState.Player;
    }

    private void FixedUpdate()
    {
        base.Update();
        if (!player)
            return;

        if (_state.State != FlowState.Running || _showpiece)
        {
            ServiceLocator.Instance.Get<IAudioService>().Stop("STATUE_MOVING");
            _rigidbody.isKinematic = true;
            return;
        }
        
        Vector3 playerPos = player.transform.position;
        playerPos.y = transform.position.y;
        
        if (canMove) 
        {
            _rigidbody.isKinematic = false;
            transform.LookAt(playerPos);
            _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, player.transform.position, maxMoveSpeed * Time.fixedDeltaTime));
            if ((player.transform.position - _rigidbody.position).magnitude < THRESHOLD)
            {
                canMove = false;
                TriggerGameOver();
            }
        }
        else if (_timer > 0f)
        {
            float speed = Easings.OutExpo(Mathf.Lerp(maxMoveSpeed, 0f, 1 - _timer / decelerationTime));
            transform.LookAt(playerPos);
            _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, player.transform.position, speed * Time.fixedDeltaTime));
            _timer -= Time.fixedDeltaTime;
            if (_timer <= 0f)
            {
                ServiceLocator.Instance.Get<IAudioService>().Stop("STATUE_MOVING");
                _rigidbody.isKinematic = true;
            }
        }
    }

    private void TriggerGameOver()
    {
        GameOverPayload payload = new();
        payload.Statue = this;
        Messenger.Default.Publish(payload);
    }

    public override void Show()
    {
        base.Show();
        if (!activated) 
            activated = true;
        
        // Check if this is the first time the player sees a statue
        if (PlayerPrefs.GetInt("SEEN_STATUE") == 0)
        {
            ServiceLocator.Instance.Get<IAudioService>().Play("STATUE_APPEAR");
            PlayerPrefs.SetInt("SEEN_STATUE", 1);
            PlayerPrefs.Save();
        }

        _timer = decelerationTime;
        canMove = false;
        // Forcibly hide the statue's shadow as it has wonky geometry
        HideShadow();
        meshRenderer.enabled = true;
        transparent.gameObject.SetActiveFast(true);
        if (!_showpiece)
            ServiceLocator.Instance.Get<IAudioService>().Play("STATUE_MOVING");
    }

    public override void Hide()
    {
        base.Hide();
        meshRenderer.enabled = false;
        transparent.gameObject.SetActiveFast(false);
        if (activated) {
            canMove = true;
        }
        if (!_showpiece)
            ServiceLocator.Instance.Get<IAudioService>().Stop("STATUE_MOVING");
    }

    public void SetShowpiece()
    {
        _showpiece = true;
    }

    public bool getCanMove { get { return canMove;}}

    public bool getActivated { get { return activated;}}

}
