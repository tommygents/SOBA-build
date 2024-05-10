using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManagerUI : MonoBehaviour
{
    public int minutes;
    public int seconds;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI labelText;
    public TextMeshProUGUI waveText;
    public GameObject WaveLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO: A function that keeps the score updated
    //TODO: A function that updates the timer
    //TODO: 

    public void UpdateTimer(float _sec)
    {
        minutes = (int)(_sec / 60);
        seconds = (int)(_sec - (minutes * 60));
        string _timer;
        if (seconds < 10) {_timer = minutes.ToString() + ":0" + seconds.ToString(); }
        else {_timer = minutes.ToString() + ":" + seconds.ToString(); }
        timerText.text = _timer;
    }

    public void isWaveActiveUI(bool _waveOn)
    {
        if (_waveOn) { labelText.text = "Time Remaining:"; }
        else { labelText.text = "Wave begins in:"; }

    }

    public void waveNumberUI(int _wave)
    {
        waveText.text = _wave.ToString();
    }

    public void TurnOffWaveLayer()
    {
        WaveLayer.SetActive(false);
    }

}
