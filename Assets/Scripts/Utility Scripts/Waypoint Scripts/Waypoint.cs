using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public Waypoint[] nextWaypoints;
    public int nextWaypointIndex = 0;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        //GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual Waypoint GetNextWaypoint()
    {
        Waypoint _wp = nextWaypoints[nextWaypointIndex];
        nextWaypointIndex++;
        if (nextWaypointIndex>= nextWaypoints.Length)
        {
            nextWaypointIndex = 0;
        }
        
        //Debug.Log("Next waypoint:" + _wp.name);
        return _wp;

    }

    private void OnDrawGizmos()
    {
        // Draw a gizmo in the editor; this won't be visible in the game.
        Gizmos.DrawSphere(transform.position, 0.5f); // Adjust the size as needed.

        // Optionally, draw a line to the next waypoint to easily visualize the path.
        if (nextWaypoints.Length > 0)
        {
            foreach (var waypoint in nextWaypoints)
            {
                Gizmos.DrawLine(transform.position, waypoint.transform.position);
            }
        }
    }
}
