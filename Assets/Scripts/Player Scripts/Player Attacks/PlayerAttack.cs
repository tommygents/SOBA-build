using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int damage = 7;
    public virtual int Damage
    {
        get { return damage; }
        set { damage = value; } // You can add additional logic here
    }

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
        if (_go.GetComponent<Enemy>() != null || _go.GetComponent<EnemySpawner>() != null)
        {
            //Code here for doing damage to an enemy
            Enemy _en = _go.GetComponent<Enemy>();
            DealDamage(_en);

        }
        if (_go.GetComponent<EnemySpawner>() != null)
        {
            Debug.Log("Hitting Spawner");
            EnemySpawner _en = _go.GetComponent<EnemySpawner>();
            DealDamage(_en);
        }
    }

    public virtual void DealDamage(Enemy _enemy)
    {
        _enemy.HP -= Damage;
        Debug.Log(Damage + "Damage");
        Debug.Log("hp: " + _enemy.hp);
    }
    public virtual void DealDamage(EnemySpawner _enemy)
    {
        _enemy.HP -= Damage;
        Debug.Log(Damage + "Damage");
        Debug.Log("hp: " + _enemy.HP);
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
