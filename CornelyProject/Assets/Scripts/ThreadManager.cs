using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    private Controls _controls;

    private LineRenderer _lineRenderer;
    private List<Vector3> _points = new List<Vector3>();

    [SerializeField] private float _distanceBetweenPoints = .1f;
    [SerializeField] private GameObject _thread;

    private void OnEnable() => _controls.Gameplay.Enable();

    private void OnDisable() => _controls.Gameplay.Disable();

    private void Awake()
    {
        _controls = new Controls();

        _controls.Gameplay.MouseLeft.started += ctx => StartNewThread();
    }

    private void StartNewThread()
    {
        _lineRenderer = Instantiate(_thread).GetComponent<LineRenderer>();
        _points.Clear();
    }

    public void AddPoint(Vector3 newPoint)
    {
        if(_points.Count ==  0 || Vector3.Distance(_points[_points.Count - 1], newPoint) > _distanceBetweenPoints)
        {
            _points.Add(newPoint);
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPositions(_points.ToArray());
        }
    }
}
