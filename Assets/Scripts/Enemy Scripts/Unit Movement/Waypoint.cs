using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public Waypoint[] nextWaypoints;
    public int nextWaypointIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Waypoint getNextWaypoint()
    {
        Waypoint _wp = nextWaypoints[nextWaypointIndex];
        nextWaypointIndex++;
        if (nextWaypointIndex>= nextWaypoints.Length)
        {
            nextWaypointIndex = 0;
        }
        return _wp;

    }
}
