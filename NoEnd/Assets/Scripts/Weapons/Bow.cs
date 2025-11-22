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

    private bool _isUsing = false;
    private float _bowStrength;

    public override void MoveWeapon(Vector3 position)
    {
        if (_isUsing)
        {
            Vector3 diff = transform.position - position;
            float dist = diff.magnitude;
            float clampedDist = Mathf.Clamp(dist, 0.0f, _maxDrag);
            _bowStrength = clampedDist / _maxDrag;
            _bowMeshRenderer.SetBlendShapeWeight(0, _bowStrength * _blendShapeMax);
            _displayArrow.localPosition = new Vector3(0, 0, _maxDisplayArrowDrag * _bowStrength);
            if (dist > 0.07f)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(diff.normalized, Vector3.up),
                    Time.fixedDeltaTime * _rotateSpeedInUse
                );
            }
        }
        else
        {
            Vector3 diff = position - transform.position;
            if (diff.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(diff.normalized, Vector3.up),
                    Time.fixedDeltaTime * _rotateSpeedNotUsing
                );
            }

            transform.position = position;
        }
    }

    public override void Use()
    {
        _isUsing = true;
    }

    public override void Release()
    {
        var obj = Instantiate(_arrowPrefab, _arrowSpawnPoint.position, _arrowSpawnPoint.rotation);
        obj.GetComponent<Rigidbody>().linearVelocity = _arrowSpawnPoint.forward * _arrowSpeed * _bowStrength;
        _bowMeshRenderer.SetBlendShapeWeight(0, 0);
        _displayArrow.localPosition = new Vector3(0, 0, 0);
        _isUsing = false;
    }
}