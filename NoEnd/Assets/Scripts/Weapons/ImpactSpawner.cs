using UnityEngine;

public class ImpactSpawner : MonoBehaviour
{
    [SerializeField] private ImpactLibrary _impact;
    [SerializeField] private bool _useThisAsParent;

    public void SpawnImpact(Vector3 point, Vector3 normal, ImpactLibrary.ImpactType impactType)
    {
        int id = (int)impactType;
        var obj = Instantiate(_impact.impactPrefabs[id], point, transform.rotation);
        obj.transform.up = normal;
        if (_useThisAsParent)
        {
            obj.transform.parent = transform;
        }
    }
}