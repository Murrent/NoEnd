using System;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    private enum WeaponType
    {
        Maul,
        Bow
    }

    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private Animator _animator;
    private bool _isAttacking = false;
    private float _bowForce = 0.0f;
    private WeaponType _weaponType = WeaponType.Maul;

    private void Start()
    {
        if (_weapon)
        {
            _weapon.Equip(false);
        }

        if (_weapon is Maul)
        {
            _weaponType = WeaponType.Maul;
        }
        else if (_weapon is Bow)
        {
            _weaponType = WeaponType.Bow;
        }
    }

    void FixedUpdate()
    {
        if (_weapon)
        {
            _weapon.MoveWeapon(_weaponHolder.position);
            _weapon.SetDirection(transform.forward);
            if (_isAttacking)
            {
                _bowForce = Mathf.Clamp(_bowForce + Time.deltaTime * 3.0f, 0.0f, 3.0f);
                _weapon.SetForce(transform.forward * _bowForce);
                if (_bowForce >= 2.99f)
                {
                    _weapon.Release();
                    _isAttacking = false;
                    _bowForce = 0.0f;
                    _weapon.SetForce(Vector3.zero);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (_weapon)
        {
            _weapon?.Unequip();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_weapon)
        {
            return;
        }

        if (_weaponType == WeaponType.Maul)
        {
            _animator.SetTrigger("attack");
        }
        else if (_weaponType == WeaponType.Bow && !_isAttacking)
        {
            _isAttacking = true;
            _bowForce = 0.0f;
            _weapon.Use();
        }
    }
}