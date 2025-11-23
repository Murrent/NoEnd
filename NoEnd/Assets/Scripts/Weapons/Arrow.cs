using Interfaces;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _tip;
    private Rigidbody _rb;
    private Vector3 _prevPosition;

    void Start()
    {
        _prevPosition = _tip.position;
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Physics.Linecast(_prevPosition, _tip.position, out var hit, _layerMask, QueryTriggerInteraction.Ignore))
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