using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public int waveNumber = 0;
    public float waveDuration = 60f;
    public float breakDuration = 120f;

    public bool isWaveActive = false;
    public float timer = 0f;

    //TODO: A timer UI, UI that shows what wave number we're on

    public static event Action OnWaveEnd;
    public static event Action<int> OnWaveStart; //passes the number of the wave to delegates


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (timer <= 0f)
        {
            //TODO: Logic that switches out the timers and either starts and ends a wave.
        }
        else
        {
            timer -= Time.deltaTime;
            
        }

       //TODO: Update the GUI


    }

    

    /*
     * The Wave Manager runs the timer and the count of which wave we're on. It gives information to a UI,
     * and other components look to it to see where we're at. (Or perhaps they listen for an event.)
     * 
     * TODO: Set two timers: one for the break, and one for the wave itself.
     * TODO: A switch that tells us whether we're in wave or in break
     * TODO: A UI that lets the player track the timer
     * TODO: The wave manager lets subscribers know that a wave has started or ended, and maybe also which wave?
     */


    public void EndWave()
    {
        isWaveActive = false;
        OnWaveEnd?.Invoke();
        //TODO: Let the UI know to switch over to a break session
        //TODO: Iterate the wave number so that it's an upcoming wave and the UI knows/indicates that it's an upcoming wave
    }

    public void StartWave()
    {
        isWaveActive = true;
        OnWaveStart?.Invoke(waveNumber);
        //TODO: Let the UI know to start a wave
        //TODO: create a w
    }

    public void SwitchTimers()
    {
        if (isWaveActive)
        {
            //TODO: Switch to wave-off timer
            EndWave();

        }
        else
        {
            //TODO: switch to wave-on timer
            StartWave();

        }
    }
}
