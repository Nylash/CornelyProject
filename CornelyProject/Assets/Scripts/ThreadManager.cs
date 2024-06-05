using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : Singleton<ThreadManager>
{
    private Transform _playerTransform;
    private LineRenderer _lineRenderer;
    private List<Vector3> _points = new List<Vector3>();

    [SerializeField] private float _distanceBetweenPoints = .1f;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        AddPoint(_playerTransform.position);
    }

    private void AddPoint(Vector3 newPoint)
    {
        if(_points.Count ==  0 || Vector3.Distance(_points[_points.Count - 1], newPoint) > _distanceBetweenPoints)
        {
            _points.Add(newPoint);
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPositions(_points.ToArray());
        }
    }
}
