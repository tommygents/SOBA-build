using UnityEngine;

[CreateAssetMenu(fileName = "TurretUpgradeChargeData", menuName = "Tower Defense/Turret Upgrade Data")]
public class TurretUpgradeChargeData : ChargeData
{
    [Header("Upgrade Settings")]
    public float currentLevel = 1f;
    public float upgradeMultiplier = 1.2f;  // How much the stat changes per upgrade
    public int maxLevel = 5;
    public string statName;  // e.g., "Damage", "Fire Rate", "Range"
    public string upgradeDescription;  // e.g., "Increases damage by 20%"
    
    [SerializeField] private float initialLevel = 1f;
    

    public bool AtMaxLevel() 
    {
        if (maxLevel <= 0)
        {
            return false;
        }
        return currentLevel >= maxLevel;
    } 

    public bool CanUpgrade()
    {
        if (AtMaxLevel()) return false;
        
        return IsFull();
    }
    
    public float GetCurrentMultiplier() => Mathf.Pow(upgradeMultiplier, currentLevel - 1);
    
    public float GetFactor() => upgradeMultiplier;
    
    public void Upgrade()
    {
        if (!AtMaxLevel())
        {
            currentLevel++;
            Debug.Log("Upgrading to " + sectionName + " level: " + currentLevel);
            
        }
    }

    public override void Reset()
    {
        base.Reset();
        currentLevel = 1;
        Debug.Log("Resetting " + sectionName + " to default level: " + initialLevel);
    }

    public void ResetToDefault()
    {
        
        currentLevel = initialLevel;
        Reset(); // Calls base Reset() for charge data
    }

    // Reset when the game starts
    private void OnEnable()
    {
        ResetToDefault();
    }
} 