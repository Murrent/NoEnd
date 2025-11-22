using System;
using UnityEngine;

public class Maul : Weapon
{
    private Vector3 _velocity;
    [SerializeField] private Transform _end;

    public override void MoveWeapon(Vector3 position)
    {
        _velocity += position - transform.position;
        transform.position = position;
    }

    public override void Use()
    {
    }

    public override void Release()
    {
    }

    private void FixedUpdate()
    {
        Vector3 diff = (Vector2)(_end.position - transform.position).normalized;
        Vector3 moveAmount = Vector2.Perpendicular(diff) * _velocity * Time.fixedDeltaTime;
        _end.position = transform.position + diff * 5.0f + moveAmount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _end.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_end.position, _end.position + _velocity.normalized * 3.0f);
    }
}