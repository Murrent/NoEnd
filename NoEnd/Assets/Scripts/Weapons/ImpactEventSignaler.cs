using System;
using UnityEngine;

public class ImpactEventSignaler : MonoBehaviour
{
    public event Action<Collision> OnImpact;
    [SerializeField] private float _threshold = 0.01f;

    private void OnCollisionEnter(Collision other)
    {
        if (other.impulse.magnitude > _threshold)
        {
            OnImpact?.Invoke(other);
        }
    }
}
