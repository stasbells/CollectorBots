using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    private const int FreeResource = 6;
    private const int ReserveResource = 7;

    private Transform _transform;
    private UnitMover _mover;

    public Base HomeBase { get; private set; }
    public Resource Target { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<UnitMover>();
        _transform = transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource) && resource == Target)
            Pack(resource);
    }

    private void Update()
    {
        if (Target != null)
            _mover.Move();
    }

    public void SetTarget(Resource target)
    {
        Target = target;
        Target.gameObject.layer = ReserveResource;
    }

    public void SetHomeBase(Base homeBase)
    {
        HomeBase = homeBase;
    }

    private void Pack(Resource resource)
    {
        float bagPositionY = 1.1f;

        resource.transform.position = new(_transform.position.x, bagPositionY, _transform.position.z);
        resource.transform.parent = _transform;
    }

    public void FinishCollecting()
    {
        Target.gameObject.layer = FreeResource;
        Target.gameObject.SetActive(false);
        Target = null;

        gameObject.SetActive(false);
    }
}