using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _transform;
    private Coroutine _move;

    private void Awake()
    {
        _transform = transform;
    }

    public void Move()
    {
        if (_move != null)
            StopCoroutine(_move);

        _move = StartCoroutine(MoveTo());
    }

    private IEnumerator MoveTo()
    {
        Transform target = GetComponent<Unit>().Target.transform;
        Transform homeBase = GetComponent<Unit>().HomeBase.transform;

        while (target != null)
        {
            if (target.parent != _transform)
                _transform.LookAt(target);
            else
                _transform.LookAt(homeBase);

            _transform.position += _transform.forward * _speed * Time.deltaTime;

            yield return null;
        }
    }
}