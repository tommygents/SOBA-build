using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour
{
    [SerializeField] private Image chargeBar;
    [SerializeField] private TextMeshProUGUI chargeCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateChargeBar(float _amount, int _chargeCount)
    {
        chargeBar.fillAmount = _amount;
        chargeCount.text = _chargeCount.ToString();

    }

   
}
