using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float _totalSpawnDuration;

    [SerializeField]
    float _spawnDistance;

    int _spawnCount = 0;
    int _maxSpawnCount;

    float _spawnCooldown;
    float _spawnTimer = 0.0f;

    [SerializeField]
    GameObject[] _enemyPrefabs;

    List<GameObject> _activeEnemies = new List<GameObject>();

    bool _isSpawning = false;

    private void OnEnable()
    {
        King.OnDayBegin += EnableSpawning;
        King.OnDayEnd += DisableSpawning;
        King.OnDayEnd += DestroyAllActiveEnemies;
    }

    private void OnDisable()
    {
        King.OnDayBegin -= EnableSpawning;
        King.OnDayEnd -= DisableSpawning;
        King.OnDayEnd -= DestroyAllActiveEnemies;
    }

    private void EnableSpawning(int day)
    {
        _isSpawning = true;

        _spawnCount = 0;
        _maxSpawnCount = day * day;

        _spawnTimer = 0.0f;
        _spawnCooldown = _totalSpawnDuration / _maxSpawnCount;
    }

    private void DisableSpawning()
    {
        _isSpawning = false;
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
        if (!_isSpawning || _spawnCount >= _maxSpawnCount)
        {
            return;
        }

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0.0f)
        {
            _spawnTimer = _spawnCooldown;
            ++_spawnCount;

            Vector2 offsetXY = Random.insideUnitCircle.normalized * _spawnDistance;
            Vector3 offset = new Vector3(offsetXY.x, 0.0f, offsetXY.y);

            int randomEnemyIndex = Random.Range(0, _enemyPrefabs.Length);

            GameObject enemy = Instantiate(_enemyPrefabs[randomEnemyIndex], transform.position + offset, Quaternion.identity);
            _activeEnemies.Add(enemy);
        }
    }
}
