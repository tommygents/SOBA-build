using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Ammo : PlayerAttack
{

    public float range;
    public float speed;
    public Vector2 startingPosition;
    


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Move();
        CheckDistance();
    }


    public virtual void CheckDistance()
    {
        //TODO: if the ammo is outside the turrets radius, it self-destructs
        if (Vector2.Distance(transform.position, startingPosition) > range)
        {
            DestroyImmediate(gameObject);
        }

    }

    public virtual void MakeAmmo(float _range, float _speed, float _angle) //Used by the turret to set the range and baseSpeed after instantiation
    {
        range = _range;
        speed = _speed;

        // Calculate direction
        Vector2 direction = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));
        direction.Normalize();

        // Set velocity
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    

    public void Move()
    {
        //TODO: bullets move
    }

    public void SetDirection(float angle)
    {
        // Calculate direction
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        direction.Normalize();

        // Set velocity
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    public override void DealDamage(Enemy _enemy)
    {
        base.DealDamage(_enemy);
        Destroy(gameObject);
       
    }
    public override void DealDamage(EnemySpawner _enemy)
    {
        base.DealDamage(_enemy);
        Destroy(gameObject);
    }


    }
