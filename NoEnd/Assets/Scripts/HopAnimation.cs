using UnityEngine;

public class HopAnimation : MonoBehaviour
{
    [SerializeField]
    Transform _animationPivot;

    [SerializeField]
    float _stepsPerMeter = 3.0f;

    [SerializeField]
    float _stepHeight = 0.2f;

    Vector3 _position;
    Vector3 _previousPosition;

    float _distanceTraveled = 0.0f;

    private void Update()
    {
        _previousPosition = _position;
        _position = transform.position;

        _distanceTraveled += (_position - _previousPosition).magnitude;

        _animationPivot.localPosition = Vector3.up * Mathf.Abs(Mathf.Sin(_distanceTraveled * Mathf.PI * _stepsPerMeter)) * _stepHeight;
    }
}
