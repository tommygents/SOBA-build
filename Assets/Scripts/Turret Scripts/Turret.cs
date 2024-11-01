using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // Add static list for basic turrets
    private static List<Turret> activeBasicTurrets = new List<Turret>();

    [SerializeField] protected Ammo ammunition; //assign in the editor, this is what the turret will shoot
    [SerializeField] protected TurretDetectionRadius targetingSystem; //gets assigned in Start()
     [SerializeField] protected TurretUI turretUI;

    //Firing variables
    protected float turretRange; // how far the turret can shoot
    [SerializeField] protected float turretBulletVelocity; // how fast the bullets move
    [SerializeField] protected float rotationSpeed = 120f; //how fast the turret can rotate towards a target
    [SerializeField] protected float rangeAsMultipleOfTargetingRadius = 1.333f; //the targeting radius gets multiplied by this number to get the turret's range, passed to bullets
   

    //Cooldown variables, so the turret doesn't just vomit bullets
    [SerializeField] protected float cooldownLength = .05f; // the length between bullets
    [SerializeField] protected float ammoCost = .05f; //the amount of charge used to fire bullets
    protected float cooldownCounter = 0f;


    public Enemy target; //the target is assigned from the targetingSystem

    //Turret charging variables
    public float chargeBar = 0f; //the amount of charge the bar currently has
    protected float chargeBarMax = 10f; //the amount of charge before the bar rolls over
    
    public int chargeCount = 0; //the number of accumulated charges
    public int chargeCountMax = 10; //the most charges that it's possible to accumulate
    public float chargeBonus = .05f; //the bonus for having lots of charges
    protected float maxCharge;
    public bool isCharging = false;
    public float baseCharge = .85f; //the starting charge rate: .8 seconds for each second running
    public float sprintBonus = 1.5f;//the rate charging happens when the player is sprinting
    public bool playerEngaged = false;
    public bool playerCharging = false;

    public Player player;
    protected CameraShake cameraShake;
    public int cameraShakeInterval = 5;
    public int cameraShakeCounter = 0;

//upgrade related variables
    public ChargeBar primaryUpgradeProgressBar;
    public ChargeBar secondaryUpgradeProgressBar;
    [SerializeField] protected TurretUpgradeChargeData primaryUpgradeData;   
    [SerializeField] protected TurretUpgradeChargeData secondaryUpgradeData; 


    // Start is called before the first frame update
   protected virtual void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        
        targetingSystem = GetComponentInChildren<TurretDetectionRadius>();  
        turretUI = GetComponent<TurretUI>();
        turretUI.chargeCountNum = chargeCountMax;
        turretUI.UpdateChargeBar(ChargeBarFullPercentage(), chargeCount, false); //initialize the charge bar
        maxCharge = chargeBarMax * chargeCountMax;
        turretRange = targetingSystem.GetTargetingRadius() * rangeAsMultipleOfTargetingRadius;
        
        // Get both upgrade progress bars
        var chargeBars = GetComponentsInChildren<ChargeBar>();
        primaryUpgradeProgressBar = chargeBars[0];
        secondaryUpgradeProgressBar = chargeBars[1];
    }

    

    // Update is called once per frame
   protected virtual void Update()
    {
        //the cooldown timer for the turret
        CountDownCooldownCounter();

        #region targeting and rotation in update
        if (NeedsTarget() && EnemyInRange()) //if the turret needs a target and there's an enemy in range, get the target
        {
            GetTarget(); //gets the closest enemy
        }
        else if (target != null) //if there is a target, check to make sure it's still valid, and then shoot at it
        {
            RotateTowardsTargetPredictively();
            if (ReadyToShoot()) //check to make sure the turret is ready to shoot
            {
                Shoot();
                
            }
        }
        #endregion
    }

   

    #region rotation and shooting
    public virtual void Shoot()
    {
        
            float fireAngle = transform.eulerAngles.z;
            Ammo _ammo = Instantiate(ammunition, transform.position, Quaternion.Euler(0, 0, fireAngle));
            _ammo.MakeAmmo(turretRange, turretBulletVelocity, fireAngle);
            cameraShake.TriggerShake();
            cooldownCounter = cooldownLength;
            DecrementChargeBar(GetAmmoCost());
            turretUI.UpdateChargeBar(ChargeBarFullPercentage(), chargeCount);
        
        //if the turret is pointing at an enemy, fire at it
    }


protected virtual float GetAmmoCost()
{
    return ammoCost * (1.2f - (chargeCount * .1f));
}
protected virtual void DecrementChargeBar(float _usedcharge)
    {
        chargeBar -= _usedcharge;
        DecrementChargeCount();
    }

protected virtual void DecrementChargeCount()
    {
        if (ChargeBarEmpty())
        {

            if (chargeCount == 0)
            {
                chargeBar = 0f;
            }
            else
            {
                chargeCount--;
                chargeBar = chargeBarMax;
            }

        }
    }

  


    void RotateTowardsTarget() //rotates toward a target
    {
        if (target != null)
        {
            // Step 1: Calculate the direction to the target
            Vector2 direction = target.gameObject.transform.position - transform.position;
            direction.Normalize();

            // Step 2: Calculate the rotation towards the target
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle); // Adjust if your turret's default orientation is not up

            // Step 3: Apply the rotation smoothly
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void RotateTowardsTargetPredictively() // Update this method to rotate towards the predicted position
    {
        if (target != null)
        {
            Vector2 targetPosition = target.transform.position;
            Vector2 targetVelocity = target.velocity;
            float distance = Vector2.Distance(transform.position, targetPosition);
            float timeToHit = distance / turretBulletVelocity;

            Vector2 predictedPosition = targetPosition + targetVelocity * timeToHit;

            Vector2 direction = predictedPosition - (Vector2)transform.position;
            direction.Normalize();

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Adjust if your turret's default orientation is not up
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    #endregion



    #region charging and engagement

    public virtual void ChargeUp(bool _sprinting)
    {
        float _time = Time.deltaTime;
        if ( _sprinting)
        {
            _time *= 1.5f;
        }
        float _chargeRate = baseCharge + (chargeBonus * chargeCount); //apply charge bonus for longer charges
        chargeBar += (_time * _chargeRate); //add to the charge bar
        if (chargeBar > chargeBarMax && chargeCount < chargeCountMax) {
            chargeBar -= chargeBarMax;
            chargeCount++;
        } //iterate the charge count if there's room to grow

        else if (chargeCount == chargeCountMax) {
            if (chargeBar > chargeBarMax)
            {
                chargeBar = chargeBarMax;
            } //if the last charge is full, just keep it full
        }

        turretUI.UpdateChargeBar((chargeBar / chargeBarMax), chargeCount, _sprinting); //finally, update the UI element
    }





    #endregion

    public virtual void GetTarget()
    {
        target = targetingSystem.GetClosestEnemy();
    }

    public void ShakeCamera()
    {
        if (cameraShakeCounter == 0)
        {
            cameraShake.TriggerShake();
            cameraShakeCounter = cameraShakeInterval;
        }

        else
        {
            cameraShakeCounter--;
        }
    }

    public virtual float GetTargetingRadius()
    {
       return targetingSystem.GetTargetingRadius();
    }

public virtual void ShowTargetingArea(Transform _origin)
{

}
    public float ChargeBarFullPercentage()
    {
        return chargeBar / chargeBarMax;
    }



    protected virtual bool NeedsTarget()
    {
        return target == null || !targetingSystem.enemiesInRange.Contains(target);
    }
    protected virtual bool EnemyInRange()
    {
        return targetingSystem.enemiesInRange.Count > 0;
    }

    protected virtual bool ShootCooldownOver()
    {
        return cooldownCounter <= 0f;
    }


    protected virtual bool HasCharge()
    {
        return chargeBar > 0f || chargeCount > 0;
    }

    protected virtual bool ChargeBarEmpty()
    {
        return chargeBar <= 0f;
    }

    protected virtual void CountDownCooldownCounter()
    {
        if (!ShootCooldownOver())
        {
            cooldownCounter -= Time.deltaTime;
        }
    }

    protected virtual bool ReadyToShoot()
    {
        return ShootCooldownOver() && targetingSystem.enemiesInRange.Contains(target) && HasCharge();
    }


    public virtual void OnEntered()
    {
        // Show primary upgrade bar by default
        //primaryUpgradeProgressBar.MakeActive();
    }

    public virtual bool IteratePrimaryUpgradeProgressBar(float _time)
    {
        primaryUpgradeProgressBar.MakeActive();
        if (primaryUpgradeData.CanUpgrade()) return false;
        
        primaryUpgradeProgressBar.IncrementChargeAmount(_time);
        return primaryUpgradeProgressBar.IsFull();
    }

    public virtual bool IterateSecondaryUpgradeProgressBar(float _time)
    {
        secondaryUpgradeProgressBar.MakeActive();
        if (secondaryUpgradeData.CanUpgrade()) return false;
        
        secondaryUpgradeProgressBar.IncrementChargeAmount(_time);
        return secondaryUpgradeProgressBar.IsFull();
    }

    public virtual void PrimaryUpgrade()
    {
        if (primaryUpgradeData.CanUpgrade())
        {
            primaryUpgradeData.Upgrade();
            primaryUpgradeProgressBar.ResetChargeAmount();
            
            // Apply the upgrade effect (implement in derived classes)
            ApplyPrimaryUpgrade();
            UpdateInstructionsTextWithUpgrades();
        }
    }

    public virtual void SecondaryUpgrade()
    {
        if (secondaryUpgradeData.CanUpgrade())
        {
            secondaryUpgradeData.Upgrade();
            secondaryUpgradeProgressBar.ResetChargeAmount();
            
            // Apply the upgrade effect (implement in derived classes)
            ApplySecondaryUpgrade();
            UpdateInstructionsTextWithUpgrades();
        }
    }

    // These methods should be overridden in derived classes to apply specific upgrades
    protected virtual void ApplyPrimaryUpgrade()
    {
        // Only apply to basic turrets
        if (GetType() == typeof(Turret))
        {
            float cooldownMultiplier = primaryUpgradeData.GetFactor();
            foreach (Turret turret in activeBasicTurrets)
            {
                turret.cooldownLength = turret.cooldownLength / cooldownMultiplier;
            }
        }
    }

    protected virtual void ApplySecondaryUpgrade()
    {
        // Only apply to basic turrets
        if (GetType() == typeof(Turret))
        {
            float detectionRadiusMultiplier = secondaryUpgradeData.GetFactor();
            foreach (Turret turret in activeBasicTurrets)
            {
                turret.targetingSystem.SetTargetingRadius(turret.targetingSystem.GetTargetingRadius() * detectionRadiusMultiplier);
            }
            player.SetNewBasicTurretTargetingRadius(targetingSystem.GetTargetingRadius() * detectionRadiusMultiplier);
            Debug.Log("New basic turret targeting radius: " + (targetingSystem.GetTargetingRadius() * detectionRadiusMultiplier));
        }
    }

    protected virtual void AlternateSecondaryUpgrade()
    {
        // Only apply to basic turrets
        if (GetType() == typeof(Turret))
        {
            float _sprintBonusFactor = secondaryUpgradeData.GetFactor();
            foreach (Turret turret in activeBasicTurrets)
            {
                turret.sprintBonus += _sprintBonusFactor;
            }
        }
    }
 
     #region debugging

    public void FillMeterDebugging()
    {
        chargeBar = chargeBarMax;
        chargeCount = chargeCountMax;
    }

#endregion

    // Add OnEnable/OnDisable for cleanup
    protected virtual void OnEnable()
    {
        RegisterTurret();
    }

    protected virtual void OnDisable()
    {
        UnregisterTurret();
    }

    public virtual void UpdateInstructionsTextWithUpgrades()
    {
        string _primaryUpgradeText = primaryUpgradeData.sectionName;
        string _secondaryUpgradeText = secondaryUpgradeData.sectionName;
        if (primaryUpgradeData.AtMaxLevel())
        {
            InstructionsUIManager.Instance.pullText.SetText(_primaryUpgradeText, "Maxed Out!");
        }
        else
        {
            InstructionsUIManager.Instance.pullText.SetText(_primaryUpgradeText, "Upgrade");
        }
        if (secondaryUpgradeData.AtMaxLevel())
        {
            InstructionsUIManager.Instance.pushText.SetText(_secondaryUpgradeText, "Maxed Out!");
        }
        else
        {
            InstructionsUIManager.Instance.pushText.SetText(_secondaryUpgradeText, "Upgrade");
        }
        InstructionsUIManager.Instance.pullText.ShowSecondaryText();
        InstructionsUIManager.Instance.pushText.ShowSecondaryText();
    }

    protected virtual void RegisterTurret()
    {
        // Only register as basic turret if this is actually a basic turret (not a derived type)
        if (GetType() == typeof(Turret))
        {
            activeBasicTurrets.Add(this);
        }
    }

    protected virtual void UnregisterTurret()
    {
        if (GetType() == typeof(Turret))
        {
            activeBasicTurrets.Remove(this);
        }
    }
}

