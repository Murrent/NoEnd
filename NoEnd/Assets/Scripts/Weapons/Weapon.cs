using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void MoveWeapon(Vector3 position);
    public abstract void Equip(bool isPlayer);
    public abstract void Unequip();
    public abstract bool IsEquipped();
    public abstract void Use();
    public abstract void Release();
    public abstract void SetForce(Vector3 force);
    public abstract void SetDirection(Vector3 direction);
}
