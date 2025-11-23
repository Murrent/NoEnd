using UnityEngine;

public class ImpactSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _impact;
    [SerializeField] private bool _useThisAsParent;

    public void SpawnImpact(Vector3 point, Vector3 normal)
    {
        var obj = Instantiate(_impact, point + normal, transform.rotation);
        obj.transform.up = normal;
        if (_useThisAsParent)
        {
            obj.transform.parent = transform;
        }
    }
}