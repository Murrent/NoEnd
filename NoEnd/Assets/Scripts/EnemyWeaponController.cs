using System;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private Animator _animator;
    private bool _isAttacking = false;

    private void Start()
    {
        _weapon.Equip(false);
    }

    void FixedUpdate()
    {
        if (_weapon)
        {
            _weapon.MoveWeapon(_weaponHolder.position);
        }
    }

    private void OnDestroy()
    {
        _weapon.Unequip();
    }

    private void OnTriggerStay(Collider other)
    {
        _animator.SetTrigger("attack");
    }
}
