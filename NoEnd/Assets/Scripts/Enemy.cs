using Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Interfaces;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] Transform _target;
    
    Rigidbody _rigidbody;

    //Health
    [SerializeField] int healthPoints = 6;
    private bool _isDead = false;
    
    //Walking
    [SerializeField] Transform _animationPivot;
    [SerializeField] AnimationCurve _accelerationOverSpeed;
    [SerializeField] float _acceleration = 10f;
    [SerializeField] float _deceleration = 5f;
    [SerializeField] float _maxSpeed = 5f;

    Vector3 _position;
    Vector3 _previousPosition;
    float _distanceTraveled;
    
    // Chasing after cursor
    [SerializeField] Camera _camera;
    [SerializeField] InputActionReference _mousePositionReference;
    
    InputAction _mousePosition;
    Vector3 _handPosition;
    Plane _groundPlane = new Plane(Vector3.up, Vector3.zero);
    
    //vfx
    [SerializeField]
    public Color flashColor;

    [SerializeField]
    public ParticleSystem explosionParticles; 

    private float _flashSpeed = 5.0f;
    private Color _originalColor;
    MeshRenderer _meshRenderer;
    
    //sound
    private AudioSource _hitSfx; 
    private AudioSource _deathSfx; 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        if (_camera == null)
            _camera = Camera.main;
    }

    private void OnEnable()
    {
        _mousePosition = _mousePositionReference.action;
        _mousePosition.Enable();
    }

    private void OnDisable()
    {
        _mousePosition.Disable();
    }

    private void Update()
    {
        if (_rigidbody.linearVelocity.magnitude > 0.1f)
        {
            _animationPivot.forward = _rigidbody.linearVelocity.normalized;
        }

        _previousPosition = _position;
        _position = transform.position;

        Vector3 positionToPreviousPosition = _position - _previousPosition;
        _distanceTraveled += positionToPreviousPosition.magnitude;

        _animationPivot.localPosition = Vector3.up * Mathf.Abs(Mathf.Sin(_distanceTraveled * Mathf.PI * 3.0f)) * 0.2f;

        GetHandPosition();
        if (_isDead)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _handPosition);
        Vector3 direction = (_handPosition - transform.position).normalized;
        float speed = _rigidbody.linearVelocity.magnitude;

        // Always accelerate towards cursor, but less force when very close (< 0.5 units)
        float forceMult = Mathf.Clamp01(distanceToTarget / 0.5f); // Smoothly reduces force near cursor
        _rigidbody.AddForce(direction * _acceleration * _accelerationOverSpeed.Evaluate(speed / _maxSpeed) * forceMult);
    
        // Cap max speed
        if (speed > _maxSpeed)
        {
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _maxSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        // healthPoints -= damage;
        
        if ( ((healthPoints -= damage) <= 0) && !_isDead )
        {
            _isDead = true;
            Debug.Log($"{gameObject.name}: PUSSYRASCLAAT mi DIED!!");
        }
    }
    
    Vector3 GetHandPosition()
    {
        Ray ray = _camera.ScreenPointToRay(_mousePosition.ReadValue<Vector2>());

        if (_groundPlane.Raycast(ray, out var enter))
        {
            _handPosition = ray.GetPoint(enter);
        }

        return _handPosition;
    }
}