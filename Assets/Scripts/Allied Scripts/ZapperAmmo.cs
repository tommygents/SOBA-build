using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ZapperAmmo : Ammo
{

    private bool activated;
    public List<Enemy> enemiesInRange = new List<Enemy>();
    private Zapper zapper;
    private ZapperCounterpart zapperCounterpart;
    [SerializeField] private float movementPenalty = 0.5f;


    // Start is called before the first frame update
   protected override void Start()
    {
        if (enemiesInRange == null)
        {
            enemiesInRange = new List<Enemy>();
        }
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
            if (debug)
            {
                Debug.Log("Enemy in range");
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            enemiesInRange.Remove(other.GetComponent<Enemy>()); //Remove the enemy from the list of enemies when they leave detection radius
            other.GetComponent<Enemy>().SetMovementPenalty(0f);
            if (debug)
            {
                Debug.Log("Enemy out of range");
            }
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

    public void SetZapperCounterpart(ZapperCounterpart _zapperCounterpart)
    {
        zapperCounterpart = _zapperCounterpart;
    }

    public override void Initialize()
{

}
    public void SetMovementPenalty(float _penalty)
    {
        movementPenalty = _penalty;
    }


    #region debugging
    [SerializeField] private bool debug = false;

    #endregion

}
