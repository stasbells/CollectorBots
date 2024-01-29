using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(Scaner))]
[RequireComponent (typeof(ObjectPool))]
public class Base : MonoBehaviour
{
    [SerializeField] private Unit _unitTamplate;
    [SerializeField] private int _unitsCount;
    [SerializeField] private TMP_Text _text;

    private float _score;

    private Scaner _scaner;
    private Transform _transform;
    private ObjectPool _objectPool;

    public event UnityAction<GameObject> Delivered;

    private void Awake()
    {
        _score = 0;
        _transform = transform;
        _scaner = GetComponent<Scaner>();
        _objectPool = GetComponent<ObjectPool>();
        CriateUnits();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Unit unit) && unit.GetComponentInChildren<Resource>() != null)
        {
            Delivered?.Invoke(unit.Target.gameObject);
            unit.FinishCollecting();

            _text.text = (++_score).ToString();
        }
    }

    private void Update()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (_objectPool.TryGetObject(out GameObject unit))
        {
            unit.transform.position = _transform.position;
            unit.GetComponent<Unit>().SetTarget(_scaner.GetNearestResource());
            unit.SetActive(true);
        }
    }

    private void CriateUnits()
    {
        _objectPool.Initialize(_unitTamplate.gameObject, _unitsCount);

        foreach(var unit in _objectPool.Pool)
            unit.GetComponent<Unit>().SetHomeBase(gameObject.GetComponent<Base>());
    }
}