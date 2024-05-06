using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RengeGames.HealthBars;


public class TurretUI : MonoBehaviour
{
    
    //[SerializeField] private TextMeshProUGUI chargeCount;
    [SerializeField] private Color[] colors;
    [SerializeField] private RadialSegmentedHealthBar chargeBar;
    [SerializeField] private RadialSegmentedHealthBar unitBar;
    [SerializeField] public int chargeCountNum;
    private RadialSegmentedHealthBar[] healthBars;
    [SerializeField] private Color sprintColor;
    // Start is called before the first frame update
    void Start()
    {
        
        chargeBar = GetComponent<RadialSegmentedHealthBar>();
        unitBar = GetComponentsInChildren<RadialSegmentedHealthBar>()[1];

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateChargeBar(float _amount, int _chargeCount)
    {
        chargeBar.PulseActivationThreshold.Value = 0;
        //chargeBar.fillAmount = _amount;
        chargeBar.SetPercent(_amount);
        
        unitBar.SetRemovedSegments(chargeCountNum - _chargeCount);
        chargeBar.InnerColor.Value = colors[_chargeCount];

    }

    public void UpdateChargeBar(float _amount, int _chargeCount, bool _sprinting)
    {
        chargeBar.PulseActivationThreshold.Value = _sprinting ? 1 : 0;
        //chargeBar.fillAmount = _amount;
        chargeBar.SetPercent(_amount);

        unitBar.SetRemovedSegments(chargeCountNum - _chargeCount);
        chargeBar.InnerColor.Value = colors[_chargeCount];

        


    }
}
