using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _container;

    private List<GameObject> _pool = new();
    protected int _capacity;

    protected void Initialize(GameObject gameObject)
    {
        for (int i = 0; i < _capacity; i++)
        {
            GameObject spawned = Instantiate(gameObject, _container.transform);

            spawned.SetActive(false);
            _pool.Add(spawned);
        }
    }

    protected void ResetParentTransform(GameObject item)
    {
        item.transform.parent = _container.transform;
    }

    protected bool TryGetObject(out GameObject resault)
    {
        resault = _pool.FirstOrDefault(gameObject => gameObject.activeSelf == false);

        return resault != null;
    }

    protected int GetActiveObjectsCount()
    {
        int count = 0;

        foreach (var item in _pool)
        {
            if (item.activeSelf == true)
                count++;
        }

        return count;
    }
}