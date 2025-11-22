using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    Camera _camera;

    [SerializeField]
    InputActionReference _mousePositionReference;
    InputAction _mousePosition;

    [SerializeField]
    InputActionReference _mousePressReference;
    InputAction _mousePress;


    [SerializeField] private Weapon _weapon;

    bool _isDragging = false;

    Vector3 _startDragPosition;
    Vector3 _handPosition;

    Plane _groundPlane = new Plane(Vector3.down, Vector3.zero);

    private void OnEnable()
    {
        if (_mousePress is not null)
        {
            _mousePress.performed += OnPress;
            _mousePress.canceled += OnReleased;
        }
    }

    private void OnDisable()
    {
        if (_mousePress is not null)
        {
            _mousePress.performed -= OnPress;
            _mousePress.canceled -= OnReleased;
        }
    }

    private void Awake()
    {
        if(!_camera)
        {
            _camera = Camera.main;
            Debug.LogWarning($"No \"_camera\" is assigned to gameobject \"{gameObject.name}\", assuming main camera", this);
        }

        if (_mousePositionReference)
        {
            _mousePosition = _mousePositionReference.ToInputAction();
            _mousePosition.Enable();
        }
        else
        {
            Debug.LogError($"No \"_mousePositionReference\" input action reference is assigned to gameobject \"{gameObject.name}\"", this);
        }

        if (_mousePressReference)
        {
            _mousePress = _mousePressReference.ToInputAction();
            _mousePress.Enable();

            _mousePress.performed += OnPress;
        }
        else
        {
            Debug.LogError($"No \"_mousePressReference\" input action reference is assigned to gameobject \"{gameObject.name}\"", this);
        }
    }

    private void Update()
    {
        if (_isDragging)
        {
            Debug.DrawLine(_startDragPosition, GetHandPosition(), Color.green);
        }
    }

    private void FixedUpdate()
    {
        _weapon.MoveWeapon(GetHandPosition());
    }

    void OnPress(InputAction.CallbackContext callbackContext)
    {
        _isDragging = true;

        _startDragPosition = GetHandPosition();
    }

    void OnReleased(InputAction.CallbackContext callbackContext)
    {
        _isDragging = false;
    }

    Vector3 GetHandPosition()
    {
        Ray ray = _camera.ScreenPointToRay(_mousePosition.ReadValue<Vector2>());

        if(_groundPlane.Raycast(ray, out var enter))
        {
            _handPosition = ray.GetPoint(enter) - _camera.transform.forward;
        }

        return _handPosition;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetHandPosition(), 0.2f);
        }
    }
#endif
}
