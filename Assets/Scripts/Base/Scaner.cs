using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _step;
    [SerializeField] private LayerMask _freeResource;

    private Vector3 _center;
    private List<Collider> _hitColliders;

    private void Awake()
    {
        _hitColliders = new();
        _center = GetComponent<Transform>().position;
    }

    public Resource GetNearestResource()
    {
        float radius = _radius;

        _hitColliders.Clear();

        while (_hitColliders.Count <= 0)
            _hitColliders = Physics.OverlapSphere(_center, radius += _step, _freeResource).ToList();

        return _hitColliders.First().GetComponent<Resource>();
    }
}