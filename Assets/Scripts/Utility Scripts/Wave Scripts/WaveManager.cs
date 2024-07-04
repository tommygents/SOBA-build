using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public static WaveManager Instance;
    public int waveNumber = 1;
    public float waveDuration = 60f;
    public float breakDuration = 120f;

    public bool isWaveActive = true;
    public float timer = 0f;

    public WaveManagerUI waveUI;
    

    public static event Action OnWaveEnd;
    public static event Action<int> OnWaveStart; //passes the number of the wave to delegates
    public static event Action OnEndGame;


    public bool gameOver = false;
    
    void Awake()
    {
        if (Instance == null)
        {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        }
        else
        {
        Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (timer <= 0f && !gameOver)
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

        
        waveUI.UpdateTimer(timer);


    }

    void OnDestroy()
    {
        OnWaveEnd = null;
        OnWaveStart = null;
        OnEndGame = null;
    }


    /*
     * The Wave Manager runs the timer and the count of which wave we're on. It gives information to a UI,
     * and other components look to it to see where we're at. (Or perhaps they listen for an event.)
     * 
     * Set two timers: one for the break, and one for the wave itself.
     * A switch that tells us whether we're in wave or in break

     */


    public void EndWave()
    {
        isWaveActive = false;
        OnWaveEnd?.Invoke();
        waveNumber++;

        if (waveNumber < 4)
        {
            timer = breakDuration; //Start the break timer
            waveUI.isWaveActiveUI(isWaveActive);
            waveUI.waveNumberUI(waveNumber);
        }
        else
        {
            gameOver = true;
            waveUI.gameObject.SetActive(false);
            OnEndGame?.Invoke(); }

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

    public void JumpTimerForDebugging()
    {
        timer = 0f;
    }

    
}
