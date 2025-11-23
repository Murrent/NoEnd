using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _arrowSpawnPoint;
    [SerializeField] private Transform _displayArrow;
    [SerializeField] private Transform _texture;
    [SerializeField] private float _rotateSpeedInUse;
    [SerializeField] private float _rotateSpeedNotUsing;
    [SerializeField] private float _arrowSpeed;
    [SerializeField] private float _maxDrag;
    [SerializeField] private float _maxDisplayArrowDrag;
    [SerializeField] private SkinnedMeshRenderer _bowMeshRenderer;
    [SerializeField] private float _blendShapeMax;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider[] _colliders;

    private bool _isUsing = false;
    private float _bowStrength;
    private Vector3 _lastPosition;
    private bool _isEquipped = false;
    private bool _isEquippedByPlayer = false;
    private AudioSource _sfx;
    private int _enemyLayers = 13;
    private int _playerLayers = 12;

    private void Start()
    {
        _sfx = GetComponent<AudioSource>();
        _lastPosition = transform.position;
        
        if (_isEquipped) return;
        Unequip();
    }
    
    public override void MoveWeapon(Vector3 position)
    {
        if (_isUsing && _isEquippedByPlayer)
        {
            Vector3 diff = transform.position - position;
            SetForce(diff);
        }
        else
        {
            if (_isEquippedByPlayer)
            {
                Vector3 diff = position - transform.position;
                SetDirection(diff);
            }

            _lastPosition = transform.position;
            transform.position = position;
        }
    }

    public override void Equip(bool isPlayer)
    {
        int layer = isPlayer ? _playerLayers : _enemyLayers;
        _rb.gameObject.layer = layer;
        foreach (var attackCollider in _colliders)
        {
            attackCollider.gameObject.layer = layer;
        }
        _isEquippedByPlayer = isPlayer;
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, 1.0f, pos.z);
        transform.rotation = Quaternion.identity;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _isEquipped = true;
    }

    public override void Unequip()
    {
        _rb.gameObject.layer = _playerLayers;
        foreach (var attackCollider in _colliders)
        {
            attackCollider.gameObject.layer = _playerLayers;
        }
        _isEquippedByPlayer = false;
        _rb.constraints = RigidbodyConstraints.None;
        _rb.linearVelocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;
        _isEquipped = false;
    }

    public override bool IsEquipped()
    {
        return _isEquipped;
    }

    public override void Use()
    {
        _isUsing = true;
    }

    public override void Release()
    {
        var obj = Instantiate(_arrowPrefab, _arrowSpawnPoint.position, _arrowSpawnPoint.rotation);
        obj.GetComponent<Rigidbody>().linearVelocity = _arrowSpawnPoint.forward * _arrowSpeed * _bowStrength;
        obj.GetComponent<Arrow>().shotByPlayer = _isEquippedByPlayer;
        _bowMeshRenderer.SetBlendShapeWeight(0, 0);
        _displayArrow.localPosition = new Vector3(0, 0, 0);
        _sfx.Play();
        _isUsing = false;
    }

    public override void SetForce(Vector3 force)
    {
        float dist = force.magnitude;
        float clampedDist = Mathf.Clamp(dist, 0.0f, _maxDrag);
        _bowStrength = clampedDist / _maxDrag;
        _bowMeshRenderer.SetBlendShapeWeight(0, _bowStrength * _blendShapeMax);
        _displayArrow.localPosition = new Vector3(0, 0, _maxDisplayArrowDrag * _bowStrength);
        if (dist > 0.07f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(force.normalized, Vector3.up),
                Time.fixedDeltaTime * _rotateSpeedInUse
            );
        }
    }

    public override void SetDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction.normalized, Vector3.up),
                Time.fixedDeltaTime * _rotateSpeedNotUsing
            );
        }
    }
}