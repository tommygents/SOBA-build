using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public int waveNumber = 0;
    public float waveDuration = 60f;
    public float breakDuration = 120f;

    //TODO: A timer UI, UI that shows what wave number we're on


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*
     * The Wave Manager runs the timer and the count of which wave we're on. It gives information to a UI,
     * and other components look to it to see where we're at. (Or perhaps they listen for an event.)
     * 
     * TODO: Set two timers: one for the break, and one for the wave itself.
     * TODO: A switch that tells us whether we're in wave or in break
     * TODO: A UI that 
     */
}
