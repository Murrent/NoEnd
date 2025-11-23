using Interfaces;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    [SerializeField] private int _hp = 4;
    [SerializeField] private GameObject _spawnOnDeathPrefab;
    bool _isDead;
    public bool dead
    {
        get {  return _isDead; }
    }
    
    public void TakeDamage(int damage)
    {
        if (_isDead) return;
        _hp =  Mathf.Clamp(_hp - damage, 0, int.MaxValue);
        if (_hp <= 0)
        {
            _isDead = true;
            Instantiate(_spawnOnDeathPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
