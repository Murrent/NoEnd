using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Camera _camera;

    [SerializeField] InputActionReference _mousePositionReference;
    InputAction _mousePosition;

    [SerializeField] InputActionReference _mousePressReference;
    InputAction _mousePress;

    [SerializeField] InputActionReference _mousePressRightReference;
    InputAction _mousePressRight;

    [SerializeField] private LayerMask _pickupLayers;
    [SerializeField] private Weapon _weapon;

    bool _isDragging = false;

    Vector3 _startDragPosition;
    Vector3 _handPosition;

    Plane _groundPlane = new Plane(Vector3.down, Vector3.zero);
    
    public const float HoldPosY = 0.5f; 

    private void OnEnable()
    {
        if (_mousePress is not null)
        {
            _mousePress.performed += OnPress;
            _mousePress.canceled += OnReleased;
        }

        if (_mousePressRight is not null)
        {
            _mousePressRight.performed += OnPressRight;
            _mousePressRight.canceled += OnReleasedRight;
        }
    }

    private void OnDisable()
    {
        if (_mousePress is not null)
        {
            _mousePress.performed -= OnPress;
            _mousePress.canceled -= OnReleased;
        }

        if (_mousePressRight is not null)
        {
            _mousePressRight.performed -= OnPressRight;
            _mousePressRight.canceled -= OnReleasedRight;
        }
    }

    private void Awake()
    {
        if (!_camera)
        {
            _camera = Camera.main;
            Debug.LogWarning($"No \"_camera\" is assigned to gameobject \"{gameObject.name}\", assuming main camera",
                this);
        }

        if (_mousePositionReference)
        {
            _mousePosition = _mousePositionReference.ToInputAction();
            _mousePosition.Enable();
        }
        else
        {
            Debug.LogError(
                $"No \"_mousePositionReference\" input action reference is assigned to gameobject \"{gameObject.name}\"",
                this);
        }

        if (_mousePressReference)
        {
            _mousePress = _mousePressReference.ToInputAction();
            _mousePress.Enable();

            _mousePress.performed += OnPress;
        }
        else
        {
            Debug.LogError(
                $"No \"_mousePressReference\" input action reference is assigned to gameobject \"{gameObject.name}\"",
                this);
        }

        if (_mousePressRightReference)
        {
            _mousePressRight = _mousePressRightReference.ToInputAction();
            _mousePressRight.Enable();

            _mousePressRight.performed += OnPressRight;
        }
        else
        {
            Debug.LogError(
                $"No \"_mousePressRightReference\" input action reference is assigned to gameobject \"{gameObject.name}\"",
                this);
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
        if (_weapon)
        {
            _weapon.MoveWeapon(GetHandPosition());
        }
        else
        {
            Pickup pickup = FindPickup();
        }
    }

    Pickup FindPickup()
    {
        Ray ray = _camera.ScreenPointToRay(_mousePosition.ReadValue<Vector2>());
        if (Physics.Raycast(ray.origin, ray.direction, out var hit, 10000, _pickupLayers, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.TryGetComponent<Pickup>(out var pickup))
            {
                if (!pickup.GetWeapon().IsEquipped())
                {
                    return pickup;
                }
            }
        }

        return null;
    }

    void OnPress(InputAction.CallbackContext callbackContext)
    {
        _isDragging = true;

        _startDragPosition = GetHandPosition();
        if (_weapon)
        {
            _weapon.Use();
        }
    }

    void OnReleased(InputAction.CallbackContext callbackContext)
    {
        _isDragging = false;
        if (_weapon)
        {
            _weapon.Release();
        }
    }

    void OnPressRight(InputAction.CallbackContext callbackContext)
    {
        if (_weapon)
        {
            _weapon.Unequip();
            _weapon = null;
        }
        else
        {
            Pickup pickup = FindPickup();
            if (pickup)
            {
                _weapon = pickup.GetWeapon();
                _weapon.Equip(true);
            }
        }
    }

    void OnReleasedRight(InputAction.CallbackContext callbackContext)
    {
    }

    Vector3 GetHandPosition()
    {
        Ray ray = _camera.ScreenPointToRay(_mousePosition.ReadValue<Vector2>());

        if (_groundPlane.Raycast(ray, out var enter))
        {
            float forwardY = _camera.transform.forward.y;
            if (forwardY == 0.0f)
            {
                return _handPosition;
            }

            float oneUnitYMultiplier = HoldPosY / forwardY;
            _handPosition = ray.GetPoint(enter) + _camera.transform.forward * oneUnitYMultiplier;
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