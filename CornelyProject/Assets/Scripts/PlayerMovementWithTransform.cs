using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementWithTransform : MonoBehaviour
{
    private Controls _controls;

    private bool _mouseLeftClick;
    private Vector3 _mousePosition;
    private float _initialYPosition;

    private void OnEnable() => _controls.Gameplay.Enable();

    private void OnDisable() => _controls.Gameplay.Disable();

    private void Awake()
    {
        _controls = new Controls();

        _controls.Gameplay.MouseDirection.performed += ctx => ReadMousePosition(ctx.ReadValue<Vector2>());
        _controls.Gameplay.MouseLeft.started += ctx => _mouseLeftClick = true;
        _controls.Gameplay.MouseLeft.canceled += ctx => _mouseLeftClick = false;

        _initialYPosition = transform.position.y;
    }

    private void Update()
    {
        if (_mouseLeftClick)
        {
            transform.position = _mousePosition + new Vector3(0, _initialYPosition, 0);
            GetComponent<ThreadManager>().AddPoint(transform.position);
        }
    }

    private void ReadMousePosition(Vector2 mouseInput)
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            _mousePosition = ray.GetPoint(distance);
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
