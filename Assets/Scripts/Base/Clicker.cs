using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Clicker : MonoBehaviour
{
    private const float PlacePositionY = 0.5f;

    private Camera _camera;
    private Mouse _mouse;

    public event UnityAction<Vector3> PlaceDefined;

    void Awake()
    {
        _camera = Camera.main;
        _mouse = Mouse.current;
    }

    void Update()
    {
        if (_mouse.leftButton.wasPressedThisFrame)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(_mouse.position.ReadValue()), out RaycastHit hit) && hit.collider.GetComponent<Base>())
                hit.collider.GetComponent<Base>().TryBuyNewBase();

            if (hit.collider != null && hit.collider.GetComponent<Level>())
                PlaceDefined?.Invoke(new Vector3(hit.point.x, PlacePositionY, hit.point.z));
        }
    }
}