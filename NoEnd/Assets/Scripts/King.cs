using System.Collections;
using UnityEngine;

public class King : MonoBehaviour
{
    [SerializeField]
    GameObject _meshParent;

    [SerializeField]
    NPCMovement _npcMovement;

    [SerializeField]
    Transform _carriage;

    [SerializeField]
    float _carriageTravelDistance;

    [SerializeField]
    float _carriageTravelDuration;

    [SerializeField]
    Transform _flowerPrefab;
    Transform[] _flowers;

    [SerializeField]
    int _smellCount;

    [SerializeField]
    float _smellDuration;

    Vector3 _carriageRestPosition;

    private void Awake()
    {
        _carriageRestPosition = _carriage.position;

        _npcMovement.enabled = false;
        _meshParent.SetActive(false);

        StartCoroutine(DoSequence());
    }

    IEnumerator DoSequence()
    {
        _flowers = new Transform[_smellCount];
        for (int i = 0; i < _smellCount; ++i)
        {
            
        }

        yield return CarriageEntersScreen_Coroutine();
        _npcMovement.enabled = true;
        _meshParent.SetActive(true);
        yield return SmellFlowers_Coroutine();
        yield return GoTo_Coroutine(_carriage.position);
        _npcMovement.enabled = false;
        _meshParent.SetActive(false);
        yield return CarriageExitsScreen_Coroutine();
    }

    IEnumerator SmellFlowers_Coroutine()
    {
        for (int i = 0; i < _flowers.Length; ++i)
        {
            yield return GoTo_Coroutine(_flowers[i].position);
            yield return new WaitForSeconds(_smellDuration);
        }
    }

    IEnumerator CarriageEntersScreen_Coroutine()
    {
        Vector3 carriageStartPosition = _carriage.position + Vector3.back * _carriageTravelDistance;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / _carriageTravelDuration)
        {
            _carriage.position = Vector3.Lerp(carriageStartPosition, _carriageRestPosition, 1.0f - Mathf.Pow(1.0f - t, 4.0f));

            yield return new WaitForEndOfFrame();
        }

        _carriage.position = _carriageRestPosition;
    }

    IEnumerator CarriageExitsScreen_Coroutine()
    {
        Vector3 carriageEndPosition = _carriage.position + Vector3.forward * _carriageTravelDistance;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / _carriageTravelDuration)
        {
            _carriage.position = Vector3.Lerp(_carriageRestPosition, carriageEndPosition, Mathf.Pow(t, 4.0f));

            yield return new WaitForEndOfFrame();
        }

        _carriage.position = carriageEndPosition;
    }

    IEnumerator GoTo_Coroutine(Vector3 position)
    {
        _npcMovement.SetTargetPosition(position);
        yield return new WaitUntil(() => { return _npcMovement.IsWithinMinTargetDistance(); });
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_carriage)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_carriage.position, Vector3.forward * _carriageTravelDistance);
            Gizmos.DrawRay(_carriage.position, Vector3.back * _carriageTravelDistance);
            Gizmos.DrawSphere(_carriage.position + Vector3.forward * _carriageTravelDistance, 0.5f);
            Gizmos.DrawSphere(_carriage.position + Vector3.back * _carriageTravelDistance, 0.5f);
        }

        Gizmos.color = Color.magenta;

        for (int i = 0; i < _flowers.Length - 1; ++i)
        {    
            Gizmos.DrawLine(_flowers[i].position, _flowers[i + 1].position);   
        }

        for (int i = 0; i < _flowers.Length; ++i)
        {
            Gizmos.DrawWireSphere(_flowers[i].position, _npcMovement.GetTargetMinDistance());
        }
    }
#endif
}
