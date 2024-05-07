using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyWaypointMovement : EnemyMovement
{

    public Waypoint nextWaypoint;
    public Waypoint prevWaypoint;
    public Waypoint currentWaypoint;
    public Enemy parent;

    public int directionWaypoint = 1;
    public float speed = 1.0f;
    public float waypointTime = 0;
    public float lastWaypointTime;

    public Collider2D waypointDetector;

    // Start is called before the first frame update
    void Start()
    {
        lastWaypointTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void Move()
    {
        speed = moveSpeed;
        RotateTowardsTarget();
        Vector3 startPos = prevWaypoint.transform.position;
        Vector3 endPos = nextWaypoint.transform.position;

        // Move along the path at a designated speed
        float pathLength = Vector3.Distance(startPos, endPos);
        float totalTimeForPathSeg = pathLength / speed;
        float currentTimeOnPathSeg = Time.time - lastWaypointTime;
        waypointTime += Time.deltaTime * speed;
        parent.transform.position = Vector2.Lerp(startPos, endPos, currentTimeOnPathSeg / totalTimeForPathSeg);

      
        // Check if close enough to consider "reaching" the waypoint
        if (Vector2.Distance(parent.transform.position, endPos) < 0.1f)  // threshold distance
        {
            UpdateWaypoint(nextWaypoint.GetNextWaypoint());
        }
      
    }

    public override void UpdateWaypoint(Waypoint _wp)

    {
        if (_wp == null)
        {
            Debug.Log("at the end of the path");
            ReachEndOfPath();
        }
        else
        {
            prevWaypoint = nextWaypoint;
            nextWaypoint = _wp;
            lastWaypointTime = Time.time;
        }

    }
    protected override void RotateTowardsTarget()
    {
        if (nextWaypoint != null)
        {
            Vector2 direction = nextWaypoint.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Subtract 90 degrees to align the enemy front

            // Smooth rotation
            parent.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject _go = collision.gameObject;
        if (_go.GetComponent<Waypoint>() == nextWaypoint)
        {
            // Check the distance before updating to avoid "teleporting"
            if (Vector2.Distance(transform.position, _go.transform.position) < 0.1f) // Small threshold to trigger the transition
            {
                Waypoint _nwp = _go.GetComponent<Waypoint>().GetNextWaypoint();
                

                    UpdateWaypoint(_nwp);
                
            }
        }
    }

    public void ReachEndOfPath() //The function called when a unit reaches the end of a path
    {
        Debug.Log("Destroying object");
        Destroy(parent.gameObject);
    }

}
