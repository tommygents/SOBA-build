using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZapperAmmo : Ammo
{

    private bool activated;
    public List<Enemy> enemiesInRange;
    private Zapper zapper;
    private float movementPenalty = 0.5f;


    // Start is called before the first frame update
   protected override void Start()
    {
        
    }

    // Update is called once per frame
   protected override void Update()
    {
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i] == null)
            {
                enemiesInRange.RemoveAt(i);
                i--;
            }
            else
            {
                DealDamage(enemiesInRange[i]);
                zapper.Surge(3f);
            }
        }

        
    }

public bool IsActivated()
{
    return activated;
}

public void ResetActivated()
{
    activated = false;
}

    public override void DealDamage(Enemy _enemy)
    {
        _enemy.TakeIncrementalDamage(CalculateDamagePerFrame());
    }


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            enemiesInRange.Add(other.GetComponent<Enemy>()); //Add the enemy to the list of enemies
            other.GetComponent<Enemy>().SetMovementPenalty(0.5f);
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            enemiesInRange.Remove(other.GetComponent<Enemy>()); //Remove the enemy from the list of enemies when they leave detection radius
            other.GetComponent<Enemy>().SetMovementPenalty(0f);
        }
    }

    protected float CalculateDamagePerFrame()
    {
        return Damage * Time.deltaTime;
    }

    public void SetZapper(Zapper _zapper)
    {
        zapper = _zapper;
    }

    public void SetMovementPenalty(float _penalty)
    {
        movementPenalty = _penalty;
    }
}
