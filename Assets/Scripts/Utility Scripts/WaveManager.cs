using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public int waveNumber = 1;
    public float waveDuration = 60f;
    public float breakDuration = 120f;

    public bool isWaveActive = false;
    public float timer = 0f;

    public WaveManagerUI waveUI;
    

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
            if (isWaveActive)
            {
                
                EndWave();

            }
            else
            {
                
                StartWave();

            }
        }
        else
        {
            timer -= Time.deltaTime;
            
        }

        //TODO: Update the GUI
        waveUI.UpdateTimer(timer);


    }

    

    /*
     * The Wave Manager runs the timer and the count of which wave we're on. It gives information to a UI,
     * and other components look to it to see where we're at. (Or perhaps they listen for an event.)
     * 
     * Set two timers: one for the break, and one for the wave itself.
     * A switch that tells us whether we're in wave or in break
     * TODO: A UI that lets the player track the timer
     * The wave manager lets subscribers know that a wave has started or ended, and also which wave
     */


    public void EndWave()
    {
        isWaveActive = false;
        OnWaveEnd?.Invoke();
        waveNumber++;
        //TODO: Let the UI know to switch over to a break session
        //TODO: Make UI update to reflect that it's an upcoming wave
        timer = breakDuration; //Start the break timer
        waveUI.isWaveActiveUI(isWaveActive);
        waveUI.waveNumberUI(waveNumber);
    }

    public void StartWave()
    {
        isWaveActive = true;
        OnWaveStart?.Invoke(waveNumber);
        timer = waveDuration; //Start the wave timer
        //TODO: Let the UI know to start a wave
        waveUI.isWaveActiveUI(isWaveActive);
        waveUI.waveNumberUI(waveNumber);


    }

    
}
