using UnityEngine;

public class Maul : Weapon
{
    [SerializeField] private Rigidbody _root;
    [SerializeField] private Rigidbody _end;
    [SerializeField] private Transform _texture;

    public override void MoveWeapon(Vector3 position)
    {
        _root.MovePosition(position);
        _texture.position = position;
        _texture.rotation = Quaternion.LookRotation((_end.position - _root.position).normalized, Vector3.up);
    }

    public override void Use()
    {
    }

    public override void Release()
    {
    }
}