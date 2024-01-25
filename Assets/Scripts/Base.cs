using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Base : ObjectPool
{
    [SerializeField] private GameObject _unitTamplate;
    [SerializeField] private int _unitCount;
    [SerializeField] private float _secondsBetweenSpawn;
    [SerializeField] private TMP_Text _text;

    private float _elapsedTime = 0;
    private float _score;

    private Transform _transform;

    private void Awake()
    {
        _score = 0;
        _transform = transform;
        _capacity = _unitCount;
        Initialize(_unitTamplate);
    }

    private void Update()
    {
        OnGenerate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Unit unit) && unit.GetComponentInChildren<Resource>() != null)
        {
            unit.FinishCollecting();

            _text.text = (++_score).ToString();
        }
    }

    private Resource GetNearestResource()
    {
        List<Resource> resources = FindObjectsByType<Resource>(FindObjectsSortMode.None).ToList();
        Resource nearestResource = resources.Find(resource => resource.IsFree && resource.gameObject.activeSelf);

        foreach (Resource resource in resources)
        {
            if (Vector3.Distance(resource.transform.position, _transform.position) < Vector3.Distance(nearestResource.transform.position, _transform.position) && resource.IsFree)
                nearestResource = resource;
        }

        nearestResource.Reserv();

        return nearestResource;
    }

    public void OnGenerate()
    {
        _elapsedTime += Time.deltaTime;

        if (TryGetObject(out GameObject unit) && _elapsedTime > _secondsBetweenSpawn)
            Spawn(unit);
    }

    private void Spawn(GameObject unit)
    {
        _elapsedTime = 0;

        unit.transform.position = transform.position;
        unit.GetComponent<Unit>().SetTarget(GetNearestResource());
        unit.SetActive(true);
    }
}