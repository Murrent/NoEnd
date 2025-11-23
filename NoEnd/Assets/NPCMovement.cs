using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NPCMovement : MonoBehaviour
{
    Rigidbody _rigidbody;

    [SerializeField]
    Transform _animationPivot;

    [SerializeField]
    AnimationCurve _accelerationOverSpeed;

    [SerializeField]
    float _acceleration;

    [SerializeField, Range(0.0f, 1.0f)]
    float _deceleration;

    [SerializeField]
    float _maxSpeed;

    [SerializeField]
    float _minTargetDistance;

    Vector3 _targetPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _targetPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (_animationPivot && _rigidbody.linearVelocity.magnitude > 0.05f)
        {
            _animationPivot.forward = _rigidbody.linearVelocity.normalized;
        }
        else
        {
            _animationPivot.forward = Vector3.forward;
        }

        _rigidbody.linearVelocity *= 1.0f - _deceleration;

        bool isWithinMinTargetDistance = IsWithinMinTargetDistance();
        if (!isWithinMinTargetDistance)
        {
            // Accelerate
            _rigidbody.AddForce(GetTargetDirection() * _acceleration * _accelerationOverSpeed.Evaluate(GetCurrentSpeed() / _maxSpeed), ForceMode.Acceleration);      
        }
    }

    Vector3 GetTargetDirection()
    {
        Vector3 direction = (_targetPosition - transform.position).normalized;
        return direction;
    }

    float GetCurrentSpeed()
    {
        float speed = _rigidbody.linearVelocity.magnitude;
        return speed;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public bool IsWithinMinTargetDistance()
    {
        Vector3 from = transform.position;
        from.y = 0.0f;
        Vector3 to = _targetPosition;
        to.y = 0.0f;

        return Vector3.Distance(from, to) <= _minTargetDistance;
    }

    public void SetTargetMinDistance(float minTargetDistance)
    {
        _minTargetDistance = minTargetDistance;
    }

    public float GetTargetMinDistance()
    {
        return _minTargetDistance;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, GetTargetDirection() * 3.0f);
    }
#endif
}
