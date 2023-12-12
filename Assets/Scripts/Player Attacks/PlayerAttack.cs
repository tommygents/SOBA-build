using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public virtual int Damage { get; set; } = 7;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    #region constructors
    /*
    public PlayerAttack(float _duration)
    {

    }
    */
    #endregion

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    
    {
        GameObject _go = collision.gameObject;
        if (_go.GetComponent<Enemy>() != null)
        {
            //Code here for doing damage to an enemy
            Enemy _en = _go.GetComponent<Enemy>();
            DealDamage(_en);

        }
    }

    public virtual void DealDamage(Enemy _enemy)
    {
        _enemy.HP -= Damage;
        Debug.Log(Damage + "Damage");
        Debug.Log("HP: " + _enemy.HP);
    }

    public virtual void Initialize()
    {
        //attackStarted = true;
    }

    public virtual void Initialize(float _duration)
    {
        Initialize();
    }
}
