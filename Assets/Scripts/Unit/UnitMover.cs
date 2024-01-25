using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void Move(Resource target)
    {
        if (!target.IsTaken)
            _transform.LookAt(target.transform);
        else
            _transform.LookAt(FindFirstObjectByType<Base>().transform);

        _transform.position += _transform.forward * _speed * Time.deltaTime;
    }
}
