using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float _spawnCooldown;

    [SerializeField]
    int _spawnCount;

    float _spawnTimer = 0.0f;

    [SerializeField]
    GameObject[] _enemiePrefabs;

    List<GameObject> _activeEnemies = new List<GameObject>();

    private void OnEnable()
    {
        King.OnDayEnd += DestroyAllActiveEnemies;
    }

    private void OnDisable()
    {
        King.OnDayEnd -= DestroyAllActiveEnemies;
    }

    private void DestroyAllActiveEnemies()
    {
        foreach (var activeEnemy in _activeEnemies)
        {
            Destroy(activeEnemy);
        }
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0.0f)
        {
            _spawnTimer = _spawnCooldown;

            for (int i = 0; i < _spawnCount; ++i)
            {
                GameObject enemy = Instantiate(_enemiePrefabs[0], transform.position + Vector3.forward * 12.0f, Quaternion.identity);
                _activeEnemies.Add(enemy);
            }
        }
    }
}
