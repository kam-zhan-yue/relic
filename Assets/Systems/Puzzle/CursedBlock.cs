using System;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Point
{
    public Transform transform;
    public float waitTime;
}

public class CursedBlock : EtherealBehaviour, ISwitchable
{
    [SerializeField, TableList] private Point[] points = Array.Empty<Point>();
    [SerializeField] private float timeToMaxSpeed = 1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float decelerationDistance = 2f;

    private int _index = -1;
    private float _elapsedTime = 0f;
    private float _speed = 0f;
    private float _time;
    private Rigidbody _rb;
    private State _state = State.Waiting;
    private Point CurrentPoint => points[_index];

    private enum State
    {
        Moving = 0,
        Waiting = 1,
    }

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PreviousPoint();
    }

    protected override void Start()
    {
        base.Start();
        NextPoint();
    }

    private void NextPoint()
    {
        _index++;
        if (_index >= points.Length)
            _index = 0;
        Move();
    }

    private void PreviousPoint()
    {
        _index--;
        if (_index < 0)
            _index = points.Length - 1;
        Move();
    }

    private void Move()
    {
        _state = State.Moving;
        _elapsedTime = 0f;
        _speed = 0f;
    }

    private void Wait()
    {
        _state = State.Waiting;
        _elapsedTime = 0f;
        _speed = 0f;
    }

    private void FixedUpdate()
    {
        if (_state == State.Moving)
        {
            Vector3 difference = CurrentPoint.transform.position - _rb.position;
            float distance = difference.magnitude;
            if (distance < decelerationDistance)
                _speed = Mathf.Lerp(0f, maxSpeed, distance / decelerationDistance);
            else
            {
                if (_speed == 0f) // Insert sound effect when starting to accelerate
                {
                    ServiceLocator.Instance.Get<IAudioService>().Play("CURSED_ACC");
                }
                _speed = Mathf.Lerp(0f, maxSpeed, _elapsedTime / timeToMaxSpeed);
            }

            _rb.MovePosition(Vector3.MoveTowards(_rb.position, CurrentPoint.transform.position, _speed * Time.fixedDeltaTime));

            _elapsedTime += Time.fixedDeltaTime;
            if (distance < 0.2f)
                Wait();
        }
        else if (_state == State.Waiting)
        {
            _elapsedTime += Time.fixedDeltaTime;
            if (_elapsedTime >= CurrentPoint.waitTime)
                NextPoint();
        }
    }
}
