using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IDamageable
{
    Transform _target;

    //Health
    [SerializeField] int healthPoints = 6;
    private bool _isDead = false;

    [SerializeField]
    NPCMovement _npcMovement;

    // Chasing after cursor
    //[SerializeField] Camera _camera;
    //[SerializeField] InputActionReference _mousePositionReference;
    
    //InputAction _mousePosition;
    //Vector3 _handPosition;
    //Plane _groundPlane = new Plane(Vector3.up, Vector3.zero);
    
    //vfx
    //[SerializeField]
    //public Color flashColor;

    //[SerializeField]
    //public ParticleSystem explosionParticles; 

    //private float _flashSpeed = 5.0f;
    //private Color _originalColor;
    //MeshRenderer _meshRenderer;
    
    //sound
    //private AudioSource _hitSfx;
    //private AudioSource _deathSfx;

    private void Awake()
    {
        _target = GameObject.FindAnyObjectByType<King>().transform;
    }

    private void FixedUpdate()
    {
        _npcMovement.SetTargetPosition(_target.position);
    }

    public void TakeDamage(int damage)
    {
        // healthPoints -= damage;
        
        Debug.Log($"{gameObject.name}: MI HIT CurrentHP: {healthPoints}");
        if ((healthPoints -= damage) <= 0 && !_isDead)
        {
            _isDead = true;

            Destroy(this.gameObject);

            Debug.Log($"{gameObject.name}: PUSSYRASCLAAT mi DIED!!");
        }
    }
}