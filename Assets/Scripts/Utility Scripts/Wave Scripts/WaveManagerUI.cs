using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManagerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI labelText;
    public TextMeshProUGUI waveText;
    public GameObject WaveLayer;

    public void UpdateTimer(float _sec)
    {
        int minutes = Mathf.FloorToInt(_sec / 60F);
        int seconds = Mathf.FloorToInt(_sec - minutes * 60);
        string _timer = string.Format("{0:0}:{1:00}", minutes, seconds);
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