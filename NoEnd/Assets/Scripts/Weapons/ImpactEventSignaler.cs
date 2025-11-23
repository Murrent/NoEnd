using System;
using UnityEngine;

public class ImpactEventSignaler : MonoBehaviour
{
    public event Action<Collision> OnImpact;
    [SerializeField] private float _threshold = 25.0f;

    private void OnCollisionEnter(Collision other)
    {
        if (other.impulse.magnitude > _threshold)
        {
            OnImpact?.Invoke(other);
        }
    }
}
