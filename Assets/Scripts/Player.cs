using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{

    //[SerializeField] public InputAction moveAction;
    public Vector2 moveVector = Vector2.zero;
    public float speed = 1f;
    [SerializeField] public ControlScheme controller;
    //[SerializeField] public PlayerInput playerInput;

    public PlayerAttackSet attackSet; //AttackSet is a collection of moves that are specified elsewhere


    public bool holdingPress = false;
    public float holdingDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        attackSet = GetComponent<PlayerAttackSet>();
        //playerInput = GetComponent<PlayerInput>();
        controller = new ControlScheme();

        controller.gameplay.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //move the player according to the tilt of the RingCon
        moveVector = controller.gameplay.move.ReadValue<Vector2>();
        OnMove(controller.gameplay.move.ReadValue<Vector2>());

        //iterate holdingDuration
        if (holdingPress)
        {
            holdingDuration += Time.deltaTime;
        }
        //Check to see if any actions are currently in progress
        HeavyPress();
    }

    public void OnMove(Vector2 _vector2)
    {
        transform.Translate(_vector2 * speed * Time.deltaTime);
    }


    public void HeavyPress()
    {
        controller.gameplay.heavypush.started += ctx =>
        {
            if (ctx.interaction is PressInteraction)
            {
                Debug.Log("Button pressed"); 
                holdingPress = true;
                holdingDuration = 0f; //holdingDuration gets iterated in the Update() function
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
}
