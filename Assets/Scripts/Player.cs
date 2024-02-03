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
    public float speed = 1f;
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
    // Start is called before the first frame update
    void Start()
    {
        attackSet = GetComponent<PlayerAttackSet>();
        //playerInput = GetComponent<PlayerInput>();
        controller = new ControlScheme();

        controller.gameplay.Enable();
        healthManager = GetComponent<HealthManager>();
        actualSpeed = speed;

    }

    // Update is called once per frame
    void Update()
    {
        //move the player according to the tilt of the RingCon
        moveVector = controller.gameplay.move.ReadValue<Vector2>();
        if (invertControls) moveVector.y *= -1;
        OnMove(moveVector);

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reloads the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    public void OnMove(Vector2 _vector2)
    {
        
        transform.Translate(_vector2 * actualSpeed * Time.deltaTime);
    }

    public void Sprint()
    {
        controller.gameplay.Sprint.started += ctx =>
        {
            actualSpeed = 2f * speed;
        };
        controller.gameplay.Sprint.performed += ctx =>
        {
            actualSpeed = speed;
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
        

        controller.gameplay.lightpull.performed += ctx =>
        {
            Vector2 direction = moveVector;
            Vector3 attackPosition = transform.position + new Vector3(direction.x, direction.y, 0) * attackDistance;
            PlayerAttack _light = Instantiate(attackSet.lightPush, attackPosition, Quaternion.identity, this.gameObject.transform);
        };

        controller.gameplay.lightpush.performed += ctx =>
        {
            Vector2 direction = moveVector;
            Vector3 attackPosition = transform.position + new Vector3(direction.x, direction.y, 0) * attackDistance;
            PlayerAttack _light = Instantiate(attackSet.lightPush, attackPosition, Quaternion.identity, this.gameObject.transform);
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
}
