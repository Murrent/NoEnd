using System.Collections;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    private enum DespawnType
    {
        Instant,
        ScaleOut
    }

    [SerializeField] private float _despawnDelay;
    [SerializeField] private float _animationTime;
    [SerializeField] private DespawnType _despawnType;

    private void Start()
    {
        StartCoroutine(DespawnRoutine());
    }

    private IEnumerator DespawnRoutine()
    {
        switch (_despawnType)
        {
            case DespawnType.Instant:
            {
                yield return new WaitForSeconds(_despawnDelay);
                break;
            }
            case DespawnType.ScaleOut:
            {
                float timeUntilAnimation = Mathf.Clamp(_despawnDelay - _animationTime, 0.0f, _despawnDelay);
                yield return new WaitForSeconds(timeUntilAnimation);
                if (_animationTime == 0.0f)
                {
                    break;
                }

                float animationStartTime = Time.time;
                Vector3 scale = transform.localScale;
                float timeSinceStart = Time.time - animationStartTime;
                while (timeSinceStart < _animationTime)
                {
                    transform.localScale = scale * Mathf.Lerp(1.0f, 0.0f, timeSinceStart / _animationTime);
                    yield return null;
                    timeSinceStart = Time.time - animationStartTime;
                }

                break;
            }
        }

        Destroy(gameObject);
    }
}