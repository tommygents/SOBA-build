using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : Waypoint
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * This is the endpoint, the final waypoint in a path that
     * enemies take.
     * 
     * When enemies talk to this waypoint, they get destroyed and
     * ... maybe counted?
     * 
     */
}
