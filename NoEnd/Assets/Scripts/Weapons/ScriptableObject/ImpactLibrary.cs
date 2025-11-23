using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ImpactLibraryData", menuName = "ScriptableObjects/ImpactLibraryData", order = 1)]
public class ImpactLibrary : ScriptableObject
{
    public enum ImpactType
    {
        Blood,
        BloodBig,
        Blunt
    }

    public GameObject[] impactPrefabs;
}