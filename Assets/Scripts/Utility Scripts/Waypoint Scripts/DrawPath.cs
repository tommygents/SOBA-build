using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawPath : MonoBehaviour
{
    public float lineWidth = 0.2f;         // Default width of the path
    public Material lineMaterial;          // Material to be used for the paths
    public Color lineStartColor = Color.red; // Default start color
    public Color lineEndColor = Color.red;   // Default end color
    public float colliderWidth = 0.5f;     // Width of the collider along the path
    private int pathLayer;

    private List<Vector3[]> paths = new List<Vector3[]>();

    void Awake()
    {
        pathLayer = LayerMask.NameToLayer("PathCollider");
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

        GameObject pathObject = new GameObject("Path");
        pathObject.layer = pathLayer;
        pathObject.transform.SetParent(transform);

        LineRenderer lineRenderer = pathObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
        ConfigureLineRenderer(lineRenderer);

        // Add colliders along the path
        AddCollidersToPath(pathObject, pathPoints);
    }

    void ConfigureLineRenderer(LineRenderer lr)
    {
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.material = lineMaterial;
        lr.startColor = lineStartColor;
        lr.endColor = lineEndColor;
    }

    // Method to add colliders along the path
    private void AddCollidersToPath(GameObject pathObject, Vector3[] pathPoints)
    {
        for (int i = 1; i < pathPoints.Length; i++)
        {
            AddColliderBetweenPoints(pathObject, pathPoints[i - 1], pathPoints[i]);
        }
    }

    private void AddColliderBetweenPoints(GameObject parent, Vector3 start, Vector3 end)
    {
        GameObject colliderGameObject = new GameObject("PathCollider2D");
        colliderGameObject.layer = pathLayer;
        colliderGameObject.transform.parent = parent.transform;

        BoxCollider2D collider = colliderGameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        // Position the collider at the midpoint between start and end
        Vector3 midPoint = (start + end) / 2;
        colliderGameObject.transform.position = midPoint;

        // Calculate the distance between start and end to set the length of the box
        float lineLength = Vector3.Distance(start, end);

        // Set the size of the collider: length along the path and the specified width
        collider.size = new Vector2(lineLength, colliderWidth);

        // Calculate the angle to rotate the collider to align with the path
        Vector3 direction = (end - start).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the collider to align with the path
        colliderGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
