using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    Transform _target;

    Rigidbody _rigidbody;

    [SerializeField]
    Transform _animationPivot;

    [SerializeField]
    AnimationCurve _accelerationOverSpeed;

    [SerializeField]
    float _acceleration;

    [SerializeField]
    float _deceleration;

    [SerializeField]
    float _maxSpeed;

    [SerializeField]
    float _targetDistance;

    Vector3 _position;
    Vector3 _previousPosition;

    float _distanceTraveled;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_rigidbody.linearVelocity.magnitude > 0.1f)
        {
            _animationPivot.forward = _rigidbody.linearVelocity.normalized;
        }

        _previousPosition = _position;
        _position = transform.position;

        Vector2 positionToPreviousPosition = _position - _previousPosition;

        _distanceTraveled += positionToPreviousPosition.magnitude;

        _animationPivot.localPosition = Vector3.up * Mathf.Abs(Mathf.Sin(_distanceTraveled * Mathf.PI * 3.0f)) * 0.2f;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _target.position) > _targetDistance)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            float speed = _rigidbody.linearVelocity.magnitude;

            _rigidbody.AddForce(direction * _acceleration * _accelerationOverSpeed.Evaluate(speed / _maxSpeed), ForceMode.Acceleration);
        }
        else
        {
            Vector3 direction = (transform.position - _target.position).normalized;
            float speed = _rigidbody.linearVelocity.magnitude;

            _rigidbody.AddForce(direction * speed * _deceleration, ForceMode.Acceleration);
        }
    }
}
