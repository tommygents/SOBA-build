using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyWaypointMovement : EnemyMovement
{

    public Waypoint nextWaypoint;
    public Waypoint prevWaypoint;
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

        RotateTowardsTarget();
        Vector3 startPos = prevWaypoint.transform.position;

        Vector3 endPos = nextWaypoint.transform.position;
        
        //This section moves the thing along the path at its particular speed
        float pathLength = Vector3.Distance(startPos, endPos);
        float totalTimeforPathSeg = pathLength / speed;
        float currentTimeOnPathSeg = Time.time - lastWaypointTime;
        waypointTime += Time.deltaTime * speed;
        parent.transform.position = Vector2.Lerp(startPos, endPos, currentTimeOnPathSeg / totalTimeforPathSeg);

        //TODO: Next, it needs a "close enough" detector, which will tell it to get the next waypoint and head in that direction
    }

    public override void UpdateWaypoint(Waypoint _wp)

    {
        prevWaypoint = nextWaypoint;
        nextWaypoint= _wp;
        lastWaypointTime = Time.time;

    }
    protected override void RotateTowardsTarget()
    {
        Vector2 direction = nextWaypoint.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Subtract 90 degrees to align the enemy front

        // Smooth rotation
        parent.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject _go = collision.gameObject;
        if (_go.GetComponent<Waypoint>() == nextWaypoint)
                {
            
            Waypoint _nwp = _go.GetComponent<Waypoint>().GetNextWaypoint();
            UpdateWaypoint(_nwp);
        }
    }

}
