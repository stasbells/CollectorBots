using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Scaner))]
[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Clicker))]
public class Base : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private Flag _flag;
    [SerializeField] private int _unitsCount;
    [SerializeField] private int _maxUnitsCount;

    private Price _price = new();
    private Scaner _scaner;
    private ObjectPool _units;
    private Clicker _clicker;
    private ResourceGenerator _resourceGenerator;
    private Transform _transform;
    private int _collectedResource;
    private bool _isPaid;

    public event UnityAction<int> ScoreChanged;

    private void Awake()
    {
        _isPaid = false;
        _collectedResource = 0;
        _transform = transform;
        _scaner = GetComponent<Scaner>();
        _units = GetComponent<ObjectPool>();
        _clicker = GetComponent<Clicker>();
        _resourceGenerator = FindFirstObjectByType<ResourceGenerator>();

        CriateUnits(_maxUnitsCount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Unit unit) && unit.HomeBase == GetComponent<Base>() && unit.GetComponentInChildren<Resource>() != null)
        {
            _resourceGenerator.ResetParent(unit.Target.gameObject);
            unit.FinishCollecting();

            _collectedResource += _price.Resource;
            ScoreChanged?.Invoke(_price.Resource);
        }
    }

    private void Update()
    {
        if (_units.GetActiveObjectsCount() < _unitsCount)
            Spawn();

        if (_collectedResource >= _price.Unit && _unitsCount < _maxUnitsCount)
            TryBuyUnit();
    }

    private void OnDisable()
    {
        _clicker.PlaceDefined -= CriateNewBaseOn;
    }

    public void Spawn()
    {
        if (_units.TryGetObject(out GameObject unit))
        {
            unit.transform.position = _transform.position;
            unit.GetComponent<Unit>().SetTarget(_scaner.GetNearestResource());
            unit.SetActive(true);
        }
    }

    public void Register(Unit unit)
    {
        _units.Pool.Add(unit.gameObject);
        unit.transform.parent = _units.Container;
    }

    public void Unregister(Unit unit)
    {
        _units.Pool.Remove(unit.gameObject);
        unit.transform.parent = null;
    }

    public void TryBuyNewBase()
    {
        if (_collectedResource >= _price.Base && !_isPaid)
        {
            _collectedResource -= _price.Base;
            ScoreChanged?.Invoke(-_price.Base);
            _clicker.PlaceDefined += CriateNewBaseOn;
            _isPaid = true;
        }
    }

    private void TryBuyUnit()
    {
        _unitsCount++;
        _collectedResource -= _price.Unit;
        ScoreChanged?.Invoke(-_price.Unit);
    }

    private void CriateUnits(int count = 1)
    {
        _units.Initialize(_unit.gameObject, count);

        foreach (var unit in _units.Pool)
            unit.GetComponent<Unit>().SetHomeBase(gameObject.GetComponent<Base>());
    }

    private void CriateNewBaseOn(Vector3 place)
    {
        if (_flag.IsDestroy)
            _clicker.PlaceDefined -= CriateNewBaseOn;

        if (!_flag.IsDestroy && _flag.gameObject.activeSelf)
            _flag.transform.position = place;

        if (!_flag.IsDestroy && !_flag.gameObject.activeSelf)
        {
            CriateUnits();
            SendBuilderUnitOn(place);
        }
    }

    private void SendBuilderUnitOn(Vector3 place)
    {
        _units.TryGetObject(out GameObject builderUnit);
        Unregister(builderUnit.GetComponent<Unit>());
        builderUnit.transform.position = _transform.position;
        builderUnit.GetComponent<Unit>().SetFlag(GetInstallFlag(place));
        builderUnit.SetActive(true);
        _flag.gameObject.SetActive(true);
    }

    private Flag GetInstallFlag(Vector3 place)
    {
        return _flag = Instantiate(_flag, place, Quaternion.identity);
    }
}