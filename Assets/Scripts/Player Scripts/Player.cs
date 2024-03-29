using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    //[SerializeField] public InputAction moveAction;
    public Vector2 moveVector = Vector2.zero;
    public float baseSpeed = 1f;
    [SerializeField] private float actualSpeed;
    [SerializeField] public ControlScheme controller;
    [SerializeField] public HealthManager healthManager;
    //[SerializeField] public PlayerInput playerInput;

    public PlayerAttackSet attackSet; //AttackSet is a collection of moves that are specified elsewhere


    public bool holdingPress = false;
    public bool holdingPull = false;
    public bool inBaseZone = true;
    public float holdingDuration = 1f;
    public float attackDistance = 2f;
    public int healAmount = 5;
    public bool invertControls = true;
    public bool isRunning = false;
    public bool isSprinting = false;
    public bool isSquating = false;

    public bool isNearTurret = false;
    public bool isEngagedWithTurret = false;
    public Turret nearbyTurret = null;
    public Turret engagedTurret = null;
    public Vector3 positionBeforeEnteringTurret;
    [SerializeField] private PlayerTurretDetector turretDetector;
    [SerializeField] private PlayerBuildingPlacement buildingPlacement;

    public float dashTimer = 0f;
    public float dashDuration = .25f;
    public float dashSpeed = 3f;
    public bool isDashing = false;

//Building placement variables

    // Start is called before the first frame update
    void Start()
    {
        attackSet = GetComponent<PlayerAttackSet>();
        //playerInput = GetComponent<PlayerInput>();
        controller = new ControlScheme();

        controller.gameplay.Enable();
        healthManager = GetComponent<HealthManager>();
        actualSpeed = baseSpeed;

        turretDetector = GetComponentInChildren<PlayerTurretDetector>();
        buildingPlacement = GetComponent<PlayerBuildingPlacement>();

    }

    // Update is called once per frame
    void Update()
    {
        //move the player according to the tilt of the RingCon
        moveVector = controller.gameplay.move.ReadValue<Vector2>();
        if (invertControls) moveVector.y *= -1;
        if (!isEngagedWithTurret)
        {
            OnMove(moveVector);
        }

        //iterate holdingDuration
        if (holdingPress)
        {
            holdingDuration += Time.deltaTime;
        }
        //Check to see if any actions are currently in progress
        HeavyPress();
        LightPress();
        HeavyPull();
        Sprint();
        Squat();
        Run();


        if (isRunning && isEngagedWithTurret) //Passes the time spent running to an engaged turret to charge it up
        {
           
            float _chargeTime = Time.deltaTime;
            if (isSprinting) { _chargeTime *= 1.5f; }
            engagedTurret.ChargeUp(_chargeTime);
        }

        if (isSquating && !isEngagedWithTurret && !turretDetector.detectsTurret) //activate the build timer, so that the player builds a turret
        {
            float _chargeTime = Time.deltaTime;
            if (buildingPlacement.IterateBuildCounter(_chargeTime)) //passes the charge time to the building manager, which returns true if enough time to build a turret has passed
            {
                Turret _turret = Instantiate(buildingPlacement.turrets[buildingPlacement.activeTurretIndex], transform.position, Quaternion.identity);
                buildingPlacement.ResetBuildCounter();
            }
        }

        if (isDashing) //iterate the dash timer and then check if the dash has ended
        {
            dashTimer += Time.deltaTime;
            if (dashTimer > dashDuration)
            {
                actualSpeed = baseSpeed;
                dashTimer = 0f;
                isDashing= false;
            }
        }



        if (Input.GetKeyDown(KeyCode.R))  // Reloads the current scene
        {
           
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }



    } //END OF UPDATE FUNCTION

    public void OnMove(Vector2 _vector2)
    {
        
        transform.Translate(_vector2 * actualSpeed * Time.deltaTime);
    }

    public void Sprint()
    {
        controller.gameplay.Sprint.started += ctx =>
        {
            actualSpeed = 2f * baseSpeed;
            isRunning = true;
            isSprinting = true;
        };
        controller.gameplay.Sprint.performed += ctx =>
        {
            actualSpeed = baseSpeed;
            isRunning = false;
            isSprinting = false;
        };
    }

    public void Squat()
    {
        controller.gameplay.Squat.started += ctx =>
        {

            isSquating = true;
            

            
        };
        controller.gameplay.Squat.performed += ctx =>
        {

            isSquating = false;
            //TODO: when the player stops squatting, reset the building placement counter.

            buildingPlacement.ResetBuildCounter();


        };
    }

    public void Run()
    {
        controller.gameplay.Run.started += ctx =>
        {

            isRunning = true;
            //TODO: Set up the charging



        };
        controller.gameplay.Run.performed += ctx =>
        {

            isRunning = false;
            //TODO: Stop charging

        };
    }

    public void HeavyPress()
    {
        controller.gameplay.heavypush.started += ctx =>
        {
            if (ctx.interaction is PressInteraction)
            {
                Debug.Log("Button pressed"); 
                holdingPress = true;
                holdingDuration = 1f; //holdingDuration gets iterated in the Update() function
            }
        };

        controller.gameplay.heavypush.performed += ctx =>
        {
            Debug.Log("Button released, duration:" + holdingDuration);

            PlayerAttack _hp = Instantiate(attackSet.heavyPush, this.transform.position, Quaternion.identity, this.gameObject.transform);
;           _hp.Initialize(holdingDuration); 
            holdingPress = false;
        };
        controller.gameplay.heavypush.canceled += ctx =>
        {
            holdingPress = false;
        };

    }

    public void LightPress()
    {
        Debug.Log("light press detected");

        controller.gameplay.lightpush.performed += ctx => //when the action is performed, a complete light push
        {
            /* This is the code that formerly triggered an attack.
            Vector2 direction = moveVector;
            Vector3 attackPosition = transform.position + new Vector3(direction.x, direction.y, 0) * attackDistance;
            PlayerAttack _light = Instantiate(attackSet.lightPush, attackPosition, Quaternion.identity, this.gameObject.transform);
       */
            if (turretDetector.detectsTurret && engagedTurret == null)
            {
                EnterTurret(turretDetector.DetectedTurret());
            } 
            else if (engagedTurret != null)
            {
                ExitTurret(engagedTurret);
            }
            else 
            {
                isDashing = true;
                actualSpeed *= dashSpeed;
            }


            };

     
    }

    public void HeavyPull()
    {
        if (inBaseZone)
        {
            controller.gameplay.heavypull.started += ctx =>
            {
                
                    Debug.Log("Pull detected");
                    holdingPull = true;
                    StartCoroutine(Heal());
                
            };
            controller.gameplay.heavypull.performed += ctx =>
            {
                holdingPull = false;
                StopCoroutine(Heal());
                StopCoroutine(Heal());
            };

            controller.gameplay.heavypull.canceled += ctx =>
            {
                holdingPull = false;
                StopCoroutine(Heal());
                StopCoroutine(Heal());
            };

        }
    }
    public void Die()
    {
        //TODO: player dies, and loses
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.GetComponent<Turret>() != null)
        {
            isNearTurret = true;
            nearbyTurret = collision.collider.gameObject.GetComponent<Turret>();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.GetComponent<Turret>() != null)
        {
            isNearTurret = false;
            nearbyTurret = null;
        }
    }


    public void EnterTurret(Turret _turret)
    {
        positionBeforeEnteringTurret = this.transform.position;
        isNearTurret = false;
        isEngagedWithTurret = true;
        nearbyTurret = null;
        engagedTurret = _turret;
        this.GetComponent<SpriteRenderer>().enabled = false; // Hide player sprite
        Camera.main.transform.position = new Vector3(_turret.transform.position.x, _turret.transform.position.y, Camera.main.transform.position.z); // Center camera on turret


    }


    public void ExitTurret(Turret _turret)
    {
        isNearTurret = true;
        isEngagedWithTurret = false;
        nearbyTurret = _turret;
        engagedTurret = null; 
        this.GetComponent<SpriteRenderer>().enabled = true; // Show player sprite
        this.transform.position = positionBeforeEnteringTurret;
        positionBeforeEnteringTurret = new Vector3();// Adjust position as needed
    }
    public IEnumerator Heal()
    {
        Debug.Log("Healing");
        while (holdingPull)
        {
            Instantiate(attackSet.heavyPull, this.transform.position, Quaternion.identity, this.gameObject.transform);
            healthManager.HP += healAmount;
            yield return new WaitForSeconds(1f);
        }
    }

    #region turret and other building placement
    //TODO: Player can place turrets by squatting for long enough
    /*
     * This involves registering that a player has squatted, checking that there's no turret in the way, showing the player's squat progress,
     * and then placing a turret that the player immediately enters.
     * 
     * 
     */




    #endregion
}
