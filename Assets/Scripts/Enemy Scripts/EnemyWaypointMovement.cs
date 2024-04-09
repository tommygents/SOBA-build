using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyWaypointMovement : EnemyMovement
{

    public Waypoint nextWaypoint;
    public Waypoint prevWaypoint;

    public int directionWaypoint = 1;
    public float speed = 1.0f;
    public float waypointTime = 0;
    public float lastWaypointTime;

    public Collider2D waypointDetector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Move()
    {
        
        
        Vector3 startPos = prevWaypoint.transform.position;

        Vector3 endPos = nextWaypoint.transform.position;
        
        //This section moves the thing along the path at its particular speed
        float pathLength = Vector3.Distance(startPos, endPos);
        float totalTimeforPathSeg = pathLength / speed;
        float currentTimeOnPathSeg = Time.time - lastWaypointTime;
        waypointTime += Time.deltaTime * speed;
        transform.position = Vector2.Lerp(startPos, endPos, currentTimeOnPathSeg / totalTimeforPathSeg);

        //TODO: Next, it needs a "close enough" detector, which will tell it to get the next waypoint and head in that direction
    }

    public void UpdateWaypoint(Waypoint _wp)

    {
        prevWaypoint = nextWaypoint;
        nextWaypoint= _wp;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject _go = collision.gameObject;
        if (_go.GetComponent<Waypoint>() == nextWaypoint)
                {
            
            Waypoint _nwp = _go.GetComponent<Waypoint>().getNextWaypoint();
            UpdateWaypoint(_nwp);
        }
    }
}
