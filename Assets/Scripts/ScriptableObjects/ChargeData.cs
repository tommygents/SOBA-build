using UnityEngine;

[CreateAssetMenu(fileName = "ChargeData", menuName = "Tower Defense/Charge Data")]
public class ChargeData : ScriptableObject
{
    public float currentCharge;
    public float maxCharge = 1f;
    public Color chargeColor;
    public string sectionName;
    public string statusText;
    
    public virtual void Reset()
    {
        currentCharge = 0f;
    }

    public virtual void ResetChargeAmount()
    {
        currentCharge = 0f;
    }

    public bool IsFull()
    {
        return currentCharge >= maxCharge;
    }

    public float GetChargePercentage()
    {
        return currentCharge / maxCharge;
    }
}
