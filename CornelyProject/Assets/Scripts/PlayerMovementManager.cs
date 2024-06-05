using UnityEngine;

public class PlayerMovementManager : Singleton<PlayerMovementManager>
{
    private Controls _controls;
    private CharacterController _characterController;
    private Animator _animator;

    private Vector2 _mouseDirection;
    private bool _mouseLeftClick;

    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _rotationSpeed = .075f;
    [SerializeField] private float _gravity = 9.83f;

    private void OnEnable() => _controls.Gameplay.Enable();

    private void OnDisable() => _controls.Gameplay.Disable();

    private void Awake()
    {
        _controls = new Controls();

        _controls.Gameplay.MouseDirection.performed += ctx => ReadMouseDirection(ctx.ReadValue<Vector2>());
        _controls.Gameplay.MouseLeft.started += ctx => _mouseLeftClick = true;
        _controls.Gameplay.MouseLeft.canceled += ctx => _mouseLeftClick = false;

        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_mouseLeftClick)
        {
            _characterController.Move(new Vector3(_mouseDirection.x, -_gravity, _mouseDirection.y) * _movementSpeed * Time.deltaTime);
        }

        _animator.SetFloat("InputX", Mathf.MoveTowards(_animator.GetFloat("InputX"), _mouseDirection.x, _rotationSpeed));
        _animator.SetFloat("InputY", Mathf.MoveTowards(_animator.GetFloat("InputY"), _mouseDirection.y, _rotationSpeed));
    }

    private void ReadMouseDirection(Vector2 mouseInput)
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance) - transform.position;
            _mouseDirection = new Vector2(target.x, target.z).normalized;
        }
    }
}
