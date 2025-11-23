using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class Arrow : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMaskEnemy;
    [SerializeField] private LayerMask _layerMaskPlayer;
    [SerializeField] private Transform _tip;
    private Rigidbody _rb;
    private Vector3 _prevPosition;
    public bool shotByPlayer = false;

    void Start()
    {
        _prevPosition = _tip.position;
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var layers = _layerMaskPlayer;
        if (shotByPlayer)
        {
            layers = _layerMaskEnemy;
        }
        if (Physics.Linecast(_prevPosition, _tip.position, out var hit, layers, QueryTriggerInteraction.Ignore))
        {
            transform.position = hit.point - _tip.TransformVector(_tip.localPosition);
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.SetParent(hit.transform);
            if (hit.transform.TryGetComponent(out ImpactSpawner impactSpawner))
            {
                impactSpawner.SpawnImpact(hit.point, hit.normal, ImpactLibrary.ImpactType.Blood);
            }

            if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(2);
            }

            Destroy(_rb);
            Destroy(this);
            return;
        }

        _prevPosition = _tip.position;
    }
}