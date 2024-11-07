using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChargeBar : MonoBehaviour
{
    [SerializeField] private ChargeData chargeData;
    
    public Color chargeColor => chargeData.chargeColor;
    public string sectionName => chargeData.sectionName;
    public string statusName => chargeData.statusText;

    public void SetChargeAmount(float _amount)
    {
        chargeData.currentCharge = Mathf.Min(_amount * chargeData.maxCharge, chargeData.maxCharge);
    }

    public bool IncrementChargeAmount(float _amount)
    {
        chargeData.currentCharge = Mathf.Min(chargeData.currentCharge + (_amount), chargeData.maxCharge);
        return chargeData.IsFull();
        //REmoved  '* chargeData.maxCharge' from the brackets 2 lines up; not sure what it was doing there.
    }

    public void MakeActive()
    {
        ChargeBarUIManager.Instance.SwitchTo(this);
    }

    public bool IsFull() 
    {
       
        return chargeData.IsFull();
    }

    public void ResetChargeAmount()
    {
        chargeData.ResetChargeAmount();
    }

    public float GetChargeAmount()
    {
        
        return chargeData.GetChargePercentage();
    }

    public bool AtMaxLevel()
    {
        if (chargeData.GetType() == typeof(TurretUpgradeChargeData))
        {
            return ((TurretUpgradeChargeData)chargeData).AtMaxLevel();
        }
        return false;
    }   
}
