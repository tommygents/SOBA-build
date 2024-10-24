using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChargeBar : MonoBehaviour
{
    public Color chargeColor;
    public string sectionName;
    public string statusName;
    private float chargeAmount;
    

    // Start is called before the first frame update
  
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetChargeAmount(float _amount)
    {
        chargeAmount = _amount;
        if(IsFull())
        {
            chargeAmount = 1f;
        }
    }

    public bool IncrementChargeAmount(float _amount)
    {
        chargeAmount += _amount;
        if(IsFull())
        {
            chargeAmount = 1f;
            return true;
            //TODO: some of these roll over, some of them don't.
        }
        return false;
    }

    public void MakeActive()
    {
        ChargeBarUIManager.Instance.SwitchTo(this);
        ChargeBarUIManager.Instance.FillChargeBarImage(chargeAmount);
    }

    

    public bool IsFull()
    {
        return chargeAmount >= 1f;
    }

    public void ResetChargeAmount()
    {
        chargeAmount = 0f;
    }

    public float GetChargeAmount()
    {
        return chargeAmount;
    }
}
