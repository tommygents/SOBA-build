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


    private int moveSpeed = 3;
    public virtual int MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }


    public int HP;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        //Called when an enemy's HP gets to zero
        Destroy(this.gameObject);
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
