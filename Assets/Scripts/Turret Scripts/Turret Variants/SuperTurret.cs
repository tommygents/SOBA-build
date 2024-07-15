using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperTurret : Turret
{
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
