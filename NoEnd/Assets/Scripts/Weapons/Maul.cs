using UnityEngine;

public class Maul : Weapon
{
    [SerializeField] private Rigidbody _root;
    [SerializeField] private Rigidbody _end;
    [SerializeField] private ConfigurableJoint _configurableJoint;
    [SerializeField] private ImpactEventSignaler _impactEventSignaler;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, Player.HoldPosY, transform.position.z);
        Unequip();
    }

    private void OnEnable()
    {
        _impactEventSignaler.OnImpact += OnImpact;
    }

    private void OnDisable()
    {
        _impactEventSignaler.OnImpact -= OnImpact;
    }

    public override void MoveWeapon(Vector3 position)
    {
        _root.MovePosition(position);
    }

    public override void Equip()
    {
        Vector3 localPos = _end.transform.localPosition;
        _end.transform.localPosition = new Vector3(localPos.x, 0.0f, localPos.z);
        _end.transform.rotation = Quaternion.identity;
        _end.constraints = RigidbodyConstraints.FreezePositionY;
        _configurableJoint.connectedBody = _end;
    }

    public override void Unequip()
    {
        _end.constraints = RigidbodyConstraints.None;
        _configurableJoint.connectedBody = null;
    }

    public override void Use()
    {
    }

    public override void Release()
    {
    }

    private void OnImpact(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ImpactSpawner impactSpawner))
        {
            impactSpawner.SpawnImpact(collision.contacts[0].point, collision.contacts[0].normal);
        }
    }
}