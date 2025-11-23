using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void MoveWeapon(Vector3 position);
    public abstract void Equip();
    public abstract void Unequip();
    public abstract void Use();
    public abstract void Release();
}
