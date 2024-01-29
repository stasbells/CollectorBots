using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _container;

    public List<GameObject> Pool { get; private set; } = new();

    public void Initialize(GameObject gameObject, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject spawned = Instantiate(gameObject, _container.transform);

            spawned.SetActive(false);

            Pool.Add(spawned);
        }
    }

    public void ResetParent(GameObject item)
    {
        item.transform.parent = _container.transform;
    }

    public bool TryGetObject(out GameObject resault)
    {
        resault = Pool.FirstOrDefault(gameObject => gameObject.activeSelf == false);

        return resault != null;
    }

    public int GetActiveObjectsCount()
    {
        return Pool.Count(item => item.activeSelf);
    }
}