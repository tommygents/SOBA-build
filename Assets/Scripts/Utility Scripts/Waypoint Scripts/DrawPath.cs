using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawPath : MonoBehaviour
{
    public float lineWidth = 0.2f;         // Default width of the path
    public Material lineMaterial;          // Material to be used for the paths
    public Color lineStartColor = Color.red; // Default start color
    public Color lineEndColor = Color.red;   // Default end color

    private List<Vector3[]> paths = new List<Vector3[]>();

    void Start()
    {
        WaypointSpawner spawner = GetComponent<WaypointSpawner>();

        if (spawner != null)
        {
            List<Vector3> initialPathPoints = new List<Vector3>();
            GeneratePath(spawner, initialPathPoints);
            foreach (var path in paths)
            {
                DrawWaypointPath(path);
            }
        }
        else
        {
            Debug.LogError("DrawPath script must be attached to a WaypointSpawner object.");
        }
    }

    void GeneratePath(Waypoint currentWaypoint, List<Vector3> currentPath)
    {
        currentPath.Add(currentWaypoint.transform.position);

        if (currentWaypoint.nextWaypoints.Length == 0)
        {
            paths.Add(currentPath.ToArray()); // Finalize this path
        }
        else
        {
            foreach (Waypoint nextWaypoint in currentWaypoint.nextWaypoints)
            {
                if (nextWaypoint != null)
                {
                    GeneratePath(nextWaypoint, new List<Vector3>(currentPath));
                }
            }
        }
    }

    void DrawWaypointPath(Vector3[] pathPoints)
    {
        if (pathPoints.Length == 0)
            return;

        LineRenderer lineRenderer = new GameObject("Path").AddComponent<LineRenderer>();
        lineRenderer.transform.SetParent(transform);
        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
        ConfigureLineRenderer(lineRenderer);
    }

    void ConfigureLineRenderer(LineRenderer lr)
    {
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.material = lineMaterial;
        lr.startColor = lineStartColor;
        lr.endColor = lineEndColor;
    }
}
