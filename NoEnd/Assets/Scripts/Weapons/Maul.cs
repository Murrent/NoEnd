using System;
using UnityEngine;

public class Maul : Weapon
{
    private Vector3 _velocity;
    private Vector3 _moveVector;
    private Vector3 _diff;
    [SerializeField] private Transform _end;

    public override void MoveWeapon(Vector3 position)
    {
        Vector3 diff = position - transform.position;
        _velocity += new Vector3(diff.x, 0.0f, diff.z);
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
        _velocity = Vector3.Lerp(_velocity, Vector3.zero, Time.fixedDeltaTime);
        _diff = (_end.position - transform.position).normalized;
        Vector2 diff2D = new Vector2(_diff.x, _diff.z);
        Vector2 perp2D = Vector2.Perpendicular(diff2D);
        Vector3 perp3D = new Vector3(perp2D.x, 0.0f, perp2D.y);
        float direction = Vector3.Dot(_velocity, perp3D);
        //direction = Mathf.Clamp(direction, -1, 1);
        //direction = direction > 0 ? 1.0f : -1.0f;
        _moveVector = perp3D * (_velocity.magnitude * direction);
        _end.position = transform.position + _diff * 5.0f + _moveVector * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _end.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_end.position, _end.position + _moveVector * 3.0f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_end.position, _end.position + _velocity * 3.0f);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(_end.position, _end.position + _velocity * 3.0f);
    }
}