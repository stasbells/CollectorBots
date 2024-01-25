using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceGenerator : ObjectPool
{
    [SerializeField] private GameObject _resourceTamplate;
    [SerializeField] private Transform _spawnDistance;
    [SerializeField] private float _unspawnRadius;
    [SerializeField] private int _spawnPointsCount;

    private float _positionY = 0.5f;

    private void Awake()
    {
        _capacity = _spawnPointsCount;
        Initialize(_resourceTamplate);
    }

    private void Update()
    {
        StartSpawn();
        ResetResources();
    }

    private void StartSpawn()
    {
        while (TryGetObject(out GameObject resource))
            RandomSpawn(resource);
    }

    private void ResetResources()
    {
        List<Resource> resources = FindObjectsByType<Resource>(FindObjectsSortMode.None).ToList();

        foreach (var resource in resources)
        {
            if (resource.transform.parent == null)
                ResetParentTransform(resource.gameObject);
        }
    }

    private bool CheckSpawnDistance(Vector3 spawnPoint)
    {
        if (Vector3.Distance(spawnPoint, FindFirstObjectByType<Base>().transform.position) < _unspawnRadius)
            return false;

        return true;
    }

    private void RandomSpawn(GameObject resource)
    {
        Vector3 randomPoint = GetRandomPoint();

        if (GetActiveObjectsCount() < _spawnPointsCount && CheckSpawnDistance(randomPoint))
        {
            resource.transform.position = randomPoint;
            resource.GetComponent<Resource>().ResetState();
            resource.SetActive(true);
        }
    }

    private Vector3 GetRandomPoint()
    {
        return new Vector3(Random.Range(-_spawnDistance.position.x, _spawnDistance.position.x),
            _positionY, Random.Range(-_spawnDistance.position.z, _spawnDistance.position.z));
    }
}