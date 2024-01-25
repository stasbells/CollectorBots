using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    private float _bag = 1.1f;
    private Vector3 _bagPosition;
    private Transform _transform;
    private UnitMover _mover;

    public Resource Target { get; private set; }

    private void Start()
    {
        _mover = GetComponent<UnitMover>();
        _transform = transform;
    }

    private void Update()
    {
        _mover.Move(Target);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource) && resource == Target)
            Pack(resource);
    }

    public void SetTarget(Resource target)
    {
        Target = target;
    }

    private void Pack(Resource resource)
    {
        _bagPosition = _transform.position;
        _bagPosition.y = _bag;
        resource.Take();
        resource.transform.position = _bagPosition;
        resource.transform.parent = _transform;
    }

    public void FinishCollecting()
    {
        Target.gameObject.transform.parent = null;
        Target.gameObject.SetActive(false);

        Target = null;
        gameObject.SetActive(false);
    }
}
