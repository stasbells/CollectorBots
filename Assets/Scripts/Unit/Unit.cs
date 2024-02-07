using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Base _baseTamplate;
    [SerializeField] private LayerMask _freeResource;

    private const int FreeResource = 6;
    private const int ReserveResource = 7;

    private Transform _transform;
    private UnitMover _mover;

    public Base HomeBase { get; private set; }
    public Resource Target { get; private set; }
    public Flag Flag { get; private set; }

    private void Awake()
    {
        _transform = transform;
        _mover = GetComponent<UnitMover>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource) && resource == Target)
            Pack(resource);

        if (collision.gameObject.TryGetComponent(out Flag flag) && flag == Flag)
            CriateBase();
    }

    private void Update()
    {
        if (Target != null || Flag != null)
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

    public void SetFlag(Flag flag)
    {
        Flag = flag;
    }

    public void FinishCollecting()
    {
        Target.gameObject.layer = FreeResource;
        Target.gameObject.SetActive(false);
        Target = null;

        gameObject.SetActive(false);
    }

    private void CriateBase()
    {
        Flag.Destroy();
        ClearPlace();

        Base newBase = Instantiate(_baseTamplate, _transform.position, Quaternion.identity);

        SetHomeBase(newBase);
        newBase.Register(this);
        gameObject.SetActive(false);
    }

    private void Pack(Resource resource)
    {
        float bagPositionY = 1.1f;

        resource.transform.position = new(_transform.position.x, bagPositionY, _transform.position.z);
        resource.transform.parent = _transform;
    }

    private void ClearPlace()
    {
        int radius = 3;
        Collider[] resources = Physics.OverlapSphere(_transform.position, radius, _freeResource);

        foreach(Collider resource in resources)
            resource.gameObject.SetActive(false);
    }
}