using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Resource _resourceTamplate;
    [SerializeField] private Transform _spawnDistance;
    [SerializeField] private float _unspawnRadius;
    [SerializeField] private int _resourcesCount;

    private float _positionY = 0.5f;
    private ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = GetComponent<ObjectPool>();
        _objectPool.Initialize(_resourceTamplate.gameObject, _resourcesCount);
    }

    private void OnEnable()
    {
        _base.Delivered += _objectPool.ResetParent;
    }

    private void Update()
    {
        Spawn();
    }

    private void OnDisable()
    {
        _base.Delivered -= _objectPool.ResetParent;
    }

    private void Spawn()
    {
        while (_objectPool.TryGetObject(out GameObject resource))
            SpawnInRandomPoint(resource);
    }

    private void SpawnInRandomPoint(GameObject resource)
    {
        Vector3 randomPoint = GetRandomPointOutsideRadius();

        if (_objectPool.GetActiveObjectsCount() < _resourcesCount)
        {
            resource.transform.position = randomPoint;
            resource.SetActive(true);
        }
    }

    private Vector3 GetRandomPointOutsideRadius()
    {
        Vector3 spawnPoint = GetRandomPoint();

        while (Vector3.Distance(spawnPoint, FindFirstObjectByType<Base>().transform.position) < _unspawnRadius)
            spawnPoint = GetRandomPoint();

        return spawnPoint;
    }

    private Vector3 GetRandomPoint()
    {
        return new Vector3(Random.Range(-_spawnDistance.position.x, _spawnDistance.position.x),
            _positionY, Random.Range(-_spawnDistance.position.z, _spawnDistance.position.z));
    }
}