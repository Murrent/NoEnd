using UnityEngine;

public class RotateY : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 100f);
    }
}
