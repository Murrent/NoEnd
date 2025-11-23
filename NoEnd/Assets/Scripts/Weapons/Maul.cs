using Interfaces;
using UnityEngine;

public class Maul : Weapon
{
    [SerializeField] private Rigidbody _root;
    [SerializeField] private Rigidbody _end;
    [SerializeField] private Collider[] _attackColliders;
    [SerializeField] private ConfigurableJoint _configurableJoint;
    [SerializeField] private ImpactEventSignaler _impactEventSignaler;
    [SerializeField] private ImpactLibrary.ImpactType _impactType;
    [SerializeField] private float _damage = 1;
    private int _enemyLayers = 13;
    private int _playerLayers = 12;
    private bool _isEquipped = false;
    
    AudioSource _audioSource;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, Player.HoldPosY, transform.position.z);
        _audioSource = GetComponent<AudioSource>();
        if (_isEquipped) return;
        Unequip();
    }

    private void OnEnable()
    {
        _impactEventSignaler.OnImpact += OnImpact;
    }

    private void OnDisable()
    {
        _impactEventSignaler.OnImpact -= OnImpact;
    }

    public override void MoveWeapon(Vector3 position)
    {
        _root.MovePosition(position);
    }

    public override void Equip(bool isPlayer)
    {
        int layer = isPlayer ? _playerLayers : _enemyLayers;
        _end.gameObject.layer = layer;
        foreach (var attackCollider in _attackColliders)
        {
            attackCollider.gameObject.layer = layer;
        }
        Vector3 localPos = _end.transform.localPosition;
        _end.transform.localPosition = new Vector3(localPos.x, 0.0f, localPos.z);
        _end.transform.rotation = Quaternion.identity;
        _end.constraints = RigidbodyConstraints.FreezePositionY;
        _configurableJoint.connectedBody = _end;
        _isEquipped = true;
    }

    public override void Unequip()
    {
        _end.gameObject.layer = _playerLayers;
        foreach (var attackCollider in _attackColliders)
        {
            attackCollider.gameObject.layer = _playerLayers;
        }
        _end.constraints = RigidbodyConstraints.None;
        _configurableJoint.connectedBody = null;
        _isEquipped = false;
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
    }

    private void OnImpact(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ImpactSpawner impactSpawner))
        {
            impactSpawner.SpawnImpact(collision.contacts[0].point, collision.contacts[0].normal, _impactType);
        }
        
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            int damage = Mathf.CeilToInt(collision.impulse.magnitude * _damage * 0.01f);
            damageable.TakeDamage(damage);
            _audioSource.Play();
            if(damageable.dead)
            {
                if (collision.gameObject.TryGetComponent(out ImpactSpawner aImpactSpawner))
                {
                    impactSpawner.SpawnImpact(collision.contacts[0].point, collision.contacts[0].normal, ImpactLibrary.ImpactType.BloodBig);
                }
            }
        }
    }
}