using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;

    public Weapon GetWeapon()
    {
        return _weapon;
    }
}
