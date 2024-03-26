using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDetectionRadius : MonoBehaviour
{
    [SerializeField] private Turret turret;
    public List<Enemy> enemiesInRange;
    private CircleCollider2D circleCollider;
    public Enemy target; //the enemy being targeted. Determined below in a function. 
    // Start is called before the first frame update
    void Start()
    {
     turret = GetComponent<Turret>();
     enemiesInRange = new List<Enemy>();
     circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            enemiesInRange.Add(other.GetComponent<Enemy>()); //Add the enemy to the list of enemies
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            enemiesInRange.Remove(other.GetComponent<Enemy>()); //Remove the enemy from the list of enemies when they leave detection radius
        }
    }

    public Enemy GetClosestEnemy() //returns the closest enemy. Worried about a null return where an enemy leaves in the middle of this check
    {
        Enemy _closestEnemy = null;
        List<Enemy> _enemies = enemiesInRange;
        foreach (Enemy _enemy in _enemies)
        {
            if (_closestEnemy == null || Vector2.Distance(transform.position, _closestEnemy.transform.position) > Vector2.Distance(transform.position, _enemy.transform.position))
            {
                _closestEnemy = _enemy;
            }
        }

        return _closestEnemy;

    }

}
