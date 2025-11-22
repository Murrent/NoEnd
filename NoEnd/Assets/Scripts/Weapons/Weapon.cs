using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void MoveWeapon(Vector3 position);
    public abstract void Use();
    public abstract void Release();
}
