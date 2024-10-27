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
        chargeData.currentCharge = Mathf.Min(chargeData.currentCharge + (_amount * chargeData.maxCharge), chargeData.maxCharge);
        return chargeData.IsFull();
    }

    public void MakeActive()
    {
        ChargeBarUIManager.Instance.SwitchTo(this);
    }

    public bool IsFull() => chargeData.IsFull();

    public void ResetChargeAmount()
    {
        chargeData.Reset();
    }

    public float GetChargeAmount()
    {
        return chargeData.GetChargePercentage();
    }
}
