using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{

   [SerializeField] private Ammo ammunition; //assign in the editor, this is what the turret will shoot
    [SerializeField] private TurretDetectionRadius targetingSystem; //gets assigned in Start()

    //Firing variables
    [SerializeField] private float turretRange; // how far the turret can shoot
    [SerializeField] private float turretBulletVelocity; // how fast the bullets move
    [SerializeField] private float rotationSpeed = 120f; //how fast the turret can rotate towards a target
    [SerializeField] private TurretUI turretUI;
    //Cooldown variables, so the turret doesn't just vomit bullets
    [SerializeField] private float cooldownLength = .05f; // the length between bullets
    private float cooldownCounter = 0f;


    public Enemy target; //the target is assigned from the targetingSystem

    //Turret charging variables
    public float chargeBar = 0f; //the amount per level of charge
    private float chargeBarMax = 10f; //the amount of charge before the bar rolls over
    
    public int chargeCount = 0; //the number of accumulated charges
    public int chargeCountMax = 10; //the most charges that it's possible to accumulate
    public float chargeBonus = .05f; //the bonus for having lots of charges
    public bool isCharging = false;
    public float baseCharge = .85f; //the starting charge rate: .8 seconds for each second running
    public float sprintBonus = 1.5f;//the rate charging happens when the player is sprinting
    public bool playerEngaged = false;
    public bool playerCharging = false;

    public Player player;



    /*
     * In the next draft, I want to fix the charge rate so that it charges at a consistent rate but burns down faster
     * 
     */
   


    // Start is called before the first frame update
    void Start()
    {
        //targetingSystem = GetComponent<TurretDetectionRadius>();
        targetingSystem = GetComponentInChildren<TurretDetectionRadius>();  
        turretUI = GetComponent<TurretUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //the cooldown timer for the turret
        if (cooldownCounter > 0f)
        {
            cooldownCounter -= Time.deltaTime;
        }




        #region targeting and rotation in update
        if ((target == null || !targetingSystem.enemiesInRange.Contains(target)) && targetingSystem.enemiesInRange.Count > 0) //checks to see if there's an enemy in range
        {
            target = targetingSystem.GetClosestEnemy(); //gets the closest enemy
        }
        else if (target != null) //if there is a target, check to make sure it's still valid, and then shoot at it
        {
            RotateTowardsTarget();
            if (cooldownCounter <= 0f && targetingSystem.enemiesInRange.Contains(target)) //check cooldown timer
            {
                Shoot();
            }
        }
        #endregion

        #region charging in update

        
        /* This code is now handled in the Player script
      //TODO: Enclose this in code that checks to make sure the player is engaged
        if (player.isRunning)
        {
            float _time = Time.deltaTime;
            if (player.isSprinting) { _time *= sprintBonus; }
            ChargeUp(_time);
        }
        */

        #endregion

        // This code will eventually be replaced by code that checks to see if the turret is occupied
        #region debugging turret 
        //this is the code for manually operating the turret
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Rotate left
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Rotate right
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Space) && cooldownCounter <= 0f)
        {
            Shoot();
        }

        #endregion


    }



    public void RotateManually()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
    {
            // Rotate left
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Rotate right
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }

    #region rotation and shooting
    public void Shoot()
    {
        if (chargeCount > 0 || chargeBar > 0f)
        {
            float fireAngle = transform.eulerAngles.z;
            Ammo _ammo = Instantiate(ammunition, transform.position, Quaternion.Euler(0, 0, fireAngle));
            _ammo.MakeAmmo(turretRange, turretBulletVelocity, fireAngle);
            cooldownCounter = cooldownLength;
            chargeBar -= cooldownLength * (1.2f - (chargeCount * .1f));
            if (chargeBar < 0f)
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
            turretUI.UpdateChargeBar((chargeBar / chargeBarMax), chargeCount);
        }
        //if the turret is pointing at an enemy, fire at it
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
    #endregion

  
    //TODO: Player can control the turret directly
    #region charging and engagement

   public void ChargeUp(float _time)
    {
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

        turretUI.UpdateChargeBar((chargeBar / chargeBarMax), chargeCount); //finally, update the UI element
    }

    //TODO: check to see if the player is running
    //TODO: infrastructure for the player to engage with the turret



    #endregion

}
