using UnityEngine;

public class RotateY : MonoBehaviour
{
    [SerializeField]
    public GameObject objectToSpawn;
    //insert object to spawn (f.ex bomb)
    [SerializeField]
    public bool spinInOppositeDirection = false;
    private float _spinSpeed = 100.0f;

    [SerializeField]
    public float spawnInterval;
    private float _timer = 0.0f;
    
    void Start()
    {
        if (spinInOppositeDirection)
        {
            _spinSpeed = -_spinSpeed;
        }
    }

    void Update()
    {
        //rotates the spawner
        transform.Rotate(Vector3.up, Time.deltaTime * _spinSpeed);
        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation);
            _timer = 0.0f;
        }
    }
}
