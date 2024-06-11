using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    private Controls _controls;

    private LineRenderer _lineRenderer;
    private List<Vector3> _points = new List<Vector3>();
    private List<LineRenderer> _threads = new List<LineRenderer>();

    [SerializeField] private float _distanceBetweenPoints = .1f;
    [SerializeField] private int _smoothingRatio = 10;
    [SerializeField] private GameObject _thread;

    private void OnEnable() => _controls.Gameplay.Enable();

    private void OnDisable() => _controls.Gameplay.Disable();

    private void Awake()
    {
        _controls = new Controls();

        _controls.Gameplay.MouseLeft.started += ctx => StartNewThread();
        _controls.Gameplay.MouseLeft.canceled += ctx => StopThread();
    }

    private void StartNewThread()
    {
        _lineRenderer = Instantiate(_thread).GetComponent<LineRenderer>();
        _points.Clear();
        _threads.Add(_lineRenderer);
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

    public void ClearAllThreads()
    {
        foreach (LineRenderer line in _threads)
        {
            Destroy(line.gameObject);
        }
        _threads.Clear();
    }

    private void StopThread()
    {
        Vector3[] positions = new Vector3[_lineRenderer.positionCount];
        _lineRenderer.GetPositions(positions);
        List<Vector3> smoothPoints = GenerateSmoothPoints(positions, _smoothingRatio);

        _lineRenderer.positionCount = smoothPoints.Count;
        _lineRenderer.SetPositions(smoothPoints.ToArray());
    }

    private List<Vector3> GenerateSmoothPoints(Vector3[] points, int segments)
    {
        List<Vector3> smoothPoints = new List<Vector3>
        {
            //Add the first point to ensure the line start at the first point
            points[0]
        };

        for (int i = 0; i < points.Length - 3; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                float t = j / (float)segments;
                smoothPoints.Add(Interpolate(points[i], points[i + 1], points[i + 2], points[i + 3], t));
            }
        }

        // Add the last point to ensure the line reaches the final point
        smoothPoints.Add(points[points.Length - 1]);

        return smoothPoints;
    }

    private Vector3 Interpolate(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        return 0.5f * (
            (2.0f * p1) +
            (-p0 + p2) * t +
            (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t2 +
            (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t3
        );
    }
}
