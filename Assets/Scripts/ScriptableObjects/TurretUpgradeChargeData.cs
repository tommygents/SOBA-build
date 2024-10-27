using UnityEngine;

[CreateAssetMenu(fileName = "TurretUpgradeChargeData", menuName = "Tower Defense/Turret Upgrade Data")]
public class TurretUpgradeChargeData : ChargeData
{
    [Header("Upgrade Settings")]
    public float currentLevel = 1f;
    public float upgradeMultiplier = 1.2f;  // How much the stat increases per upgrade
    public int maxLevel = 5;
    public string statName;  // e.g., "Damage", "Fire Rate", "Range"
    public string upgradeDescription;  // e.g., "Increases damage by 20%"
    
    public bool CanUpgrade() => currentLevel < maxLevel;
    
    public float GetCurrentMultiplier() => Mathf.Pow(upgradeMultiplier, currentLevel - 1);
    
    public void Upgrade()
    {
        if (CanUpgrade())
        {
            currentLevel++;
        }
    }

    public override void Reset()
    {
        base.Reset();
        currentLevel = 1;
    }
}
