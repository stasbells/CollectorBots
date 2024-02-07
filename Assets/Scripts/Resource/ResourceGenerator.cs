using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _resourceTamplate;
    [SerializeField] private Transform _spawnDistance;
    [SerializeField] private float _unspawnRadius;
    [SerializeField] private int _resourcesCount;
    [SerializeField] private LayerMask _base;

    private const float PositionY = 0.5f;

    private ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = GetComponent<ObjectPool>();
        _objectPool.Initialize(_resourceTamplate.gameObject, _resourcesCount);
    }

    private void Update()
    {
        Spawn();
    }

    public void ResetParent(GameObject item)
    {
        item.transform.parent = _objectPool.Container;
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

        while (Physics.OverlapSphere(spawnPoint, _unspawnRadius, _base).Length != 0)
            spawnPoint = GetRandomPoint();
        
        return spawnPoint;
    }

    private Vector3 GetRandomPoint()
    {
        return new Vector3(Random.Range(-_spawnDistance.position.x, _spawnDistance.position.x),
            PositionY, Random.Range(-_spawnDistance.position.z, _spawnDistance.position.z));
    }
}