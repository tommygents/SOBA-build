using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperTurret : Turret
{
    private static List<SuperTurret> activeSuperTurrets = new List<SuperTurret>();

    protected override void RegisterTurret()
    {
        activeSuperTurrets.Add(this);
    }

    protected override void UnregisterTurret()
    {
        activeSuperTurrets.Remove(this);
    }

    protected override void ApplyPrimaryUpgrade()
    {
        float ammoCostFactor = primaryUpgradeData.GetFactor();
        foreach (var superTurret in activeSuperTurrets)
        {
            superTurret.cooldownLength *= ammoCostFactor;
        }
    }

    protected override void ApplySecondaryUpgrade()
    {
        float damageMultiplier = secondaryUpgradeData.GetFactor();
        foreach (var superTurret in activeSuperTurrets)
        {
            superTurret.cooldownLength /= damageMultiplier;
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

    public override void ChargeUp(bool _sprinting)
    {
        if (!_sprinting)
        {
            return;
        }
        else
        {
           base.ChargeUp(false);
        }
    }

    public override void Shoot()
    {
    base.Shoot();
    }

    
    }
