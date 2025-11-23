using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class King : MonoBehaviour
{
    public static Action<int> OnDayBegin;
    public static Action OnDayEnd;

    [SerializeField]
    GameObject _meshParent;

    [SerializeField]
    NPCMovement _npcMovement;

    [SerializeField]
    Animator _bannersAndBuisines;

    [Header("Transition")]
    [SerializeField]
    float _transitionTime;

    [SerializeField]
    RawImage _transitionImage;

    [SerializeField]
    TMP_Text _transitionLabel;

    [Header("Carriage")]
    [SerializeField]
    Transform _carriage;

    [SerializeField]
    float _carriageTravelDistance;

    [SerializeField]
    float _carriageTravelDuration;

    [Header("Flowers")]
    [SerializeField]
    Transform _flowerOrigin;

    [SerializeField]
    float _flowerMinSpawnRadius;

    [SerializeField]
    float _flowerMaxSpawnRadius;

    [SerializeField]
    Transform _flowerPrefab;
    Transform[] _flowers = Array.Empty<Transform>();

    [SerializeField]
    int _smellCount;

    [SerializeField]
    float _smellDuration;

    Vector3 _carriageRestPosition;
    Vector3 _carriageStartPosition;
    Vector3 _carriageEndPosition;

    int _currentDay = 1;

    private void Awake()
    {
        _carriageRestPosition = _carriage.position;
        _carriageStartPosition = _carriage.position - _carriage.transform.forward * _carriageTravelDistance;
        _carriageEndPosition = _carriage.position + _carriage.transform.forward * _carriageTravelDistance;

        _carriage.transform.position = _carriageStartPosition;

        _npcMovement.enabled = false;
        _meshParent.SetActive(false);

        StartCoroutine(DoSequence());
    }

    IEnumerator DoSequence()
    {
        _flowers = new Transform[_smellCount];

        while (true)
        {
            for (int i = 0; i < _smellCount; ++i)
            {
                Vector2 offsetXY = Random.insideUnitCircle.normalized * (_flowerMinSpawnRadius + Random.Range(0.0f, _flowerMaxSpawnRadius - _flowerMinSpawnRadius));
                Vector3 offset = new Vector3(offsetXY.x, 0.0f, offsetXY.y);
                Vector3 position = _flowerOrigin.position + offset;

                if (_flowers[i])
                {
                    Destroy(_flowers[i].gameObject);
                }

                _flowers[i] = Instantiate(_flowerPrefab, position, Quaternion.identity);
            }

            yield return TransitionIn_Coroutine();

            _transitionLabel.text = $"Day {_currentDay}";
            ++_currentDay;

            _bannersAndBuisines.SetTrigger("banner");

            yield return CarriageEntersScreen_Coroutine();

            transform.position = _carriage.position - Vector3.up * (_carriage.position.y - 0.05f)  - _carriage.transform.right * 2.0f;

            _npcMovement.enabled = true;
            _meshParent.SetActive(true);

            OnDayBegin?.Invoke(_currentDay);

            yield return SmellFlowers_Coroutine();
            yield return GoTo_Coroutine(_carriage.position);

            _npcMovement.enabled = false;
            _meshParent.SetActive(false);

            yield return CarriageExitsScreen_Coroutine();

            yield return TransitionOut_Coroutine();

            OnDayEnd?.Invoke();
        }
    }

    IEnumerator TransitionIn_Coroutine()
    {
        yield return new WaitForSeconds(0.5f);

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / (_transitionTime - 0.5f) / 2.0f)
        {
            Color color = _transitionImage.color;
            color.a = 1.0f - t;
            _transitionImage.color = color;
            yield return new WaitForEndOfFrame();
        }

        {
            Color color = _transitionImage.color;
            color.a = 0.0f;
            _transitionImage.color = color;
        }
    }

    IEnumerator TransitionOut_Coroutine()
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / (_transitionTime - 0.5f) / 2.0f)
        {
            Color color = _transitionImage.color;
            color.a = t;
            _transitionImage.color = color;
            yield return new WaitForEndOfFrame();
        }

        {
            Color color = _transitionImage.color;
            color.a = 1.0f;
            _transitionImage.color = color;
        }

        yield return new WaitForSeconds(0.5f);
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
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / _carriageTravelDuration)
        {
            _carriage.position = Vector3.Lerp(_carriageStartPosition, _carriageRestPosition, 1.0f - Mathf.Pow(1.0f - t, 4.0f));

            yield return new WaitForEndOfFrame();
        }

        _carriage.position = _carriageRestPosition;
    }

    IEnumerator CarriageExitsScreen_Coroutine()
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / _carriageTravelDuration)
        {
            _carriage.position = Vector3.Lerp(_carriageRestPosition, _carriageEndPosition, Mathf.Pow(t, 4.0f));

            yield return new WaitForEndOfFrame();
        }

        _carriage.position = _carriageEndPosition;
    }

    IEnumerator GoTo_Coroutine(Vector3 position)
    {
        _npcMovement.SetTargetPosition(position);
        yield return new WaitUntil(() => { return _npcMovement.IsWithinMinTargetDistance(); });
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_flowerOrigin)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_flowerOrigin.position, _flowerMinSpawnRadius);
            Gizmos.DrawWireSphere(_flowerOrigin.position, _flowerMaxSpawnRadius);
        }

        if (_carriage)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_carriage.position, _carriage.transform.forward * _carriageTravelDistance);
            Gizmos.DrawRay(_carriage.position, -_carriage.transform.forward * _carriageTravelDistance);
            Gizmos.DrawSphere(_carriage.position + _carriage.transform.forward * _carriageTravelDistance, 0.5f);
            Gizmos.DrawSphere(_carriage.position - _carriage.transform.forward * _carriageTravelDistance, 0.5f);
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
