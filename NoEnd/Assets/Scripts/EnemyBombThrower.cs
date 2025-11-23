using UnityEngine;

public class EnemyBombThrower : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] private float _throwInterval = 5f;
    [SerializeField] private float _throwSpeed = 2f;
    [SerializeField] private Transform _target;
    
    private float _throwTimer;
    
    void Start()
    {
        _throwTimer = _throwInterval;
        
        if (_target == null)
        {
            _target = GameObject.FindAnyObjectByType<King>()?.transform;
        }
    }
    
    void Update()
    {
        _throwTimer -= Time.deltaTime;
        
        if (_throwTimer <= 0f)
        {
            ThrowBomb();
            _throwTimer = _throwInterval;
        }
    }
    
    void ThrowBomb()
    {
        if (_bombPrefab == null || _throwPoint == null || _target == null)
            return;
    
        Vector3 spawnPos = _throwPoint.position + transform.forward * 0.5f;
        GameObject bombObj = Instantiate(_bombPrefab, spawnPos, Quaternion.identity);
    
        bombObj.layer = LayerMask.NameToLayer("Pickable");
    
        Bomba bomb = bombObj.GetComponent<Bomba>();
    
        if (bomb != null)
        {
            Vector3 direction = transform.forward;
            bomb.ThrowFromEnemy(direction, _throwSpeed);
        
            Debug.Log($"{gameObject.name} threw bomb at {_target.name}!");
        }
    }
}