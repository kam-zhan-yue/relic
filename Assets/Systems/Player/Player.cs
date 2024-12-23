using System;
using Cinemachine;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private bool canFlashlight;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float pushForce = 1f;
    [SerializeField] private float rotateSpeed = 720f;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;
    private CinemachineVirtualCamera _virtualCamera;
    private Vector3 _input = Vector3.zero;
    private Flashlight _flashlight;
    private bool _started = false;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _flashlight = GetComponentInChildren<Flashlight>();
        _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        _mainCamera = Camera.main;
        SetAnimation(true);
        SetActive(false);
    }

    public void SetActive(bool value)
    {
        _started = value;
        _rigidbody.isKinematic = !value;
    }

    public void SetAnimation(bool value)
    {
        _animator.enabled = value;
    }

    public void SetFlashlightActive(bool active)
    {
        _flashlight.Active = active;
    }

    public void SetFlashlightAngle(float angle)
    {
        _flashlight.Torch.spotAngle = angle;
    }
    
    private void Update()
    {
        if (!_started)
            return;
        CheckFlashlight();
        Move();
        Rotate();
    }

    private void Move()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.z = Input.GetAxis("Vertical");
        _rigidbody.velocity = _input * speed;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void CheckFlashlight()
    {
        if (canFlashlight && _flashlight && Input.GetKeyDown(KeyCode.Mouse0))
        {
            _flashlight.Active = !_flashlight.Active;
            ServiceLocator.Instance.Get<IAudioService>().Play("FLASHLIGHT_SWITCH");
        }
    }

    private void Rotate()
    {
        if (canFlashlight && _flashlight && _flashlight.Active)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 position = Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundMask) ? hitInfo.point : Vector3.zero;
            if (hitInfo.collider)
            {
                Vector3 difference = position - transform.position;
                float facingAngle = Mathf.Atan2(difference.x, difference.z);
                transform.localRotation = Quaternion.Euler(0, Mathf.Rad2Deg * facingAngle, 0);
            }
        }
        else if (_input.x != 0 || _input.z != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(_input, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }
    }

    public void SetPriority(int value)
    {
        _virtualCamera.Priority = value;
    }
}
