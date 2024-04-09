using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private int maxHP = 20;
    public virtual int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    public int hp;
    public virtual int HP { get => hp; set => hp = value; }

    [SerializeField] private int moveSpeed = 3;
    public virtual int MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    [SerializeField]
    private GameObject targetGO; //The game object that the unit moves towards
    public virtual GameObject TargetGO
    {
        get { return targetGO; }
        set { targetGO = value; }
    }
    
    
    
    public SpriteRenderer spriteRenderer;
    //public bool targetInRange = false;
    public EnemyMovement movementScript;
    [SerializeField] private Player player;


    // Start is called before the first frame update
    void Start()
    {
        hp = MaxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementScript = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (hp <= 0)
        {
            Die();
        }

        if (hp < MaxHP)
        {

        }
    }


    public void Die()
    {
        //Called when an enemy's hp gets to zero
        Destroy(this.gameObject);
    }

    public void Attack(GameObject _target)
    {
        //TODO: needs some sort of UI to let us know the attack is happening

    }

    public void TargetEnter(GameObject _target)
    {
        movementScript.TargetEnter(_target);
    }
    public void TargetExit(GameObject _target)
    {
        movementScript.TargetExit(_target);
    }

    /*
     *OnCollision is, for now, geting moved to the playerattack itself. 
     * 
    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject _go = collision.gameObject;
        if (_go.GetComponent<PlayerAttack>() != null)
        {
            //Code here for taking damage from an attack
        PlayerAttack _pa = _go.GetComponent<PlayerAttack>();

        }


    }
    */
}
