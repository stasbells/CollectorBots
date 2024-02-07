using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _container;

    public Transform Container => _container.transform;

    public List<GameObject> Pool { get; private set; } = new();

    public void Initialize(GameObject gameObject, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject spawned = Instantiate(gameObject, Container);

            spawned.SetActive(false);

            Pool.Add(spawned);
        }
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