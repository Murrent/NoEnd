using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float _totalSpawnDuration;

    [SerializeField]
    float _enemySpawnDistance;

    [SerializeField]
    int _minObjectSpawnCount;
    [SerializeField]
    int _maxObjectSpawnCount;

    [SerializeField]
    float _maxObjectSpawnDistance;

    int _spawnCount = 0;
    int _maxSpawnCount;

    float _spawnCooldown;
    float _spawnTimer = 0.0f;

    [SerializeField]
    GameObject[] _enemyPrefabs;

    [SerializeField]
    GameObject[] _objectPrefabs;

    List<GameObject> _spawnedGameObjects = new List<GameObject>();

    bool _isSpawning = false;

    private void OnEnable()
    {
        King.OnDayInitialize += Initialize;
        King.OnDayBegin += EnableSpawning;
        King.OnDayEnd += DisableSpawning;
        King.OnDayEnd += DestroyAllActiveEnemies;
    }

    private void OnDisable()
    {
        King.OnDayInitialize -= Initialize;
        King.OnDayBegin -= EnableSpawning;
        King.OnDayEnd -= DisableSpawning;
        King.OnDayEnd -= DestroyAllActiveEnemies;
    }

    void Initialize()
    {
        Debug.Log("Initializing");

        int objectCount = Random.Range(_minObjectSpawnCount, _maxObjectSpawnCount);
        for (int i = 0; i < objectCount; ++i)
        {
            Vector2 offsetXY = Random.insideUnitCircle * _maxObjectSpawnDistance;
            Vector3 offset = new Vector3(offsetXY.x, 0.0f, offsetXY.y);

            int randomEnemyIndex = Random.Range(0, _objectPrefabs.Length);

            _spawnedGameObjects.Add(Instantiate(_objectPrefabs[randomEnemyIndex], transform.position + offset, Quaternion.identity));
        }
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
        foreach (var activeEnemy in _spawnedGameObjects)
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

            Vector2 offsetXY = Random.insideUnitCircle.normalized * _enemySpawnDistance;
            Vector3 offset = new Vector3(offsetXY.x, 0.0f, offsetXY.y);

            int randomEnemyIndex = Random.Range(0, _enemyPrefabs.Length);

            _spawnedGameObjects.Add(Instantiate(_enemyPrefabs[randomEnemyIndex], transform.position + offset, Quaternion.identity));
        }
    }
}
