using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Turret
{
    private static List<Cannon> activeCannonTurrets = new List<Cannon>();
    private float damageMultiplier;

    protected override void RegisterTurret()
    {
        activeCannonTurrets.Add(this);
    }

    protected override void UnregisterTurret()
    {
        activeCannonTurrets.Remove(this);
    }

    protected override void ApplyPrimaryUpgrade()
    {
        float _damageMultiplier = primaryUpgradeData.GetCurrentMultiplier();
        foreach (var cannon in activeCannonTurrets)
        {
            cannon.damageMultiplier = _damageMultiplier;
        }
    }


    protected override void ApplySecondaryUpgrade()
    {
        float fireRateMultiplier = secondaryUpgradeData.GetFactor();
        foreach (var cannon in activeCannonTurrets)
        {
            cannon.cooldownLength = cannon.cooldownLength / fireRateMultiplier;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void GetTarget()
    {
        target = targetingSystem.GetLargestEnemy();
        turretRange = Vector2.Distance(target.transform.position, transform.position);
    }

    public override void Shoot()
    {
        
            float fireAngle = transform.eulerAngles.z;
            Ammo _ammo = Instantiate(ammunition, transform.position, Quaternion.Euler(0, 0, fireAngle));
            _ammo.MakeAmmo(turretRange, turretBulletVelocity, fireAngle, damageMultiplier);
            cameraShake.TriggerShake();
            cooldownCounter = cooldownLength;
            DecrementChargeBar(GetAmmoCost());
            turretUI.UpdateChargeBar(ChargeBarFullPercentage(), chargeCount);
        
        //if the turret is pointing at an enemy, fire at it
    }
        
    
}
