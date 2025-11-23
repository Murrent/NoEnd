using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Interfaces;

public class Bomba : Weapon
{
    //explosion variables
    [SerializeField] private float fuseTimer = 3.0f;
    [SerializeField] private float explosionRadius = 1.0f;
    [SerializeField] private int damage = 5;
    private float _currentFuseTimer;
    private bool _exploded = false;
    private bool _fuseStarted = false;
    
    //throwing
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private LayerMask _enemyLayers;
    [SerializeField] private LayerMask _playerLayers;
    private bool _isEquipped = false;
    private Vector3 _lastPosition;
    
    //vfx
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private ParticleSystem explosionParticles; 

    private float _flashSpeed = 5.0f;
    private Color _originalColor;
    private MeshRenderer _meshRenderer;
    
    //sound
    private AudioSource _explosionSfx;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, Player.HoldPosY, transform.position.z);
        
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();
            
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _originalColor = _meshRenderer.material.color;
        
        _explosionSfx = GetComponent<AudioSource>();
        if (explosionParticles != null)
            explosionParticles.Stop();
        
        _currentFuseTimer = fuseTimer;
        _lastPosition = transform.position;
        
        if (_isEquipped) return;
        Unequip();
    }

    void Update()
    {
        if (_fuseStarted && _currentFuseTimer > 0)
        {
            _currentFuseTimer -= Time.deltaTime;
            
            float flashFrequency = _flashSpeed * (1f - (_currentFuseTimer / fuseTimer));
            float lerp = Mathf.PingPong(Time.time * flashFrequency, 1f);
            _meshRenderer.material.color = Color.Lerp(_originalColor, flashColor, lerp);
        }
        else if (_fuseStarted && _currentFuseTimer <= 0 && !_exploded)
        {
            Debug.Log("BOMBOCLAAAAT MI EXPLODED");
            _meshRenderer.enabled = false;
            if (explosionParticles != null)
                explosionParticles.Play();
            if (_explosionSfx != null)
                _explosionSfx.Play();
            _exploded = true;
            Explosion(explosionRadius);
        }
        
        if (_exploded && (_explosionSfx == null || !_explosionSfx.isPlaying))
        {
            Destroy(this.gameObject);
        }
    }

    void Explosion(float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var collider in hitColliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log($"EXPLODED {collider.gameObject.name}");
                damageable.TakeDamage(damage);
            }
        }
    }
    
    public void ThrowFromEnemy(Vector3 direction, float speed)
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();
        
        Debug.Log($"Throw direction: {direction}, speed: {speed}, final velocity: {direction * speed}");
    
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.linearVelocity = direction * speed;
        
        _fuseStarted = true;
    }
    
    public override void MoveWeapon(Vector3 position)
    {
        if (_isEquipped)
        {
            _lastPosition = _rigidbody.position;
            _rigidbody.MovePosition(position);
        }
    }

    public override void Equip(bool isPlayer)
    {
        gameObject.layer = isPlayer ? _playerLayers : _enemyLayers;
        
        Vector3 localPos = transform.localPosition;
        transform.localPosition = new Vector3(localPos.x, 0.0f, localPos.z);
        transform.rotation = Quaternion.identity;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        
        _isEquipped = true;
    }

    public override void Unequip()
    {
        gameObject.layer = LayerMask.NameToLayer("Pickable");
        _rigidbody.constraints = RigidbodyConstraints.None;
    
        Vector3 throwVelocity = (_rigidbody.position - _lastPosition) / Time.fixedDeltaTime;
        _rigidbody.linearVelocity = throwVelocity;
    
        _isEquipped = false;
    
        if (!_fuseStarted)
        {
            _fuseStarted = true;
        }
    }

    public override bool IsEquipped()
    {
        return _isEquipped;
    }

    public override void Use()
    {
    }

    public override void Release()
    {
        Unequip();
    }
    
    void OnDrawGizmos()
    {
        if (_exploded)
        {
            Gizmos.color = new Color(1, 0, 0, 0.8f);
            Gizmos.DrawSphere(transform.position, explosionRadius);
        }
        else if (_fuseStarted)
        {
            float fusePercent = 1f - (_currentFuseTimer / fuseTimer);
            Gizmos.color = Color.Lerp(Color.yellow, Color.red, fusePercent);
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}