using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    public EnemyFront enemyFront;
    
    public float moveSpeed = 5f;
    public float attackRange = 1f;
    public float rotationSpeed = 200f; // Degrees per second

    [SerializeField] public GameObject targetGO;
    private HealthManager targetHealthManager;
    private CircleCollider2D attackRangeCollider;
    public bool targetInRange;

    public float attackTimerLength = 2f;
    public float attackTimer = 0f;
    public int damage = 1;
    public EnemyAttack attackAnimation;


    private void Start()
    {
        attackRangeCollider = enemyFront.GetComponent<CircleCollider2D>();
        targetHealthManager= targetGO.GetComponent<HealthManager>();
    }

    void Update()
    {
        if (targetGO != null)
        {
            RotateTowardsTarget();
            MoveTowardsTarget();
            CheckAttackRange();
        }
    }

    protected virtual void RotateTowardsTarget()
    {
        Vector2 direction = targetGO.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Subtract 90 degrees to align the enemy front

        // Smooth rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public virtual void MoveTowardsTarget()
    {
        float distance = Vector2.Distance(transform.position, targetGO.transform.position);

        if (!targetInRange)
        {
            Vector2 step = moveSpeed * Time.deltaTime * (targetGO.transform.position - transform.position).normalized;
            transform.position += new Vector3(step.x, step.y, 0);
        }
    }

    void CheckAttackRange()
    {
        if (targetInRange)
        {
            // Attack logic here
            Attack();
        }
    }

    void Attack()
    {
        if (attackTimer >= attackTimerLength)
        {
            attackTimer = 0f;
            targetHealthManager.HP -= damage;
            Instantiate(attackAnimation, enemyFront.transform.position, Quaternion.identity, this.transform);
            Debug.Log("Attacking " + targetGO.name);
        }
        attackTimer += Time.deltaTime;
    }

    public void SetTarget(GameObject target)
    {
        targetGO = target;
        targetHealthManager = targetGO.GetComponent<HealthManager>();
    }

    public void TargetEnter(GameObject _target)
    {
        if (_target == targetGO)
            targetInRange = true;
    }

    public void TargetExit(GameObject _target)
    {
        if (_target == targetGO)
            targetInRange = false;
    }

    public virtual void Move()
    {


        //TODO: Next, it needs a "close enough" detector, which will tell it to get the next waypoint and head in that direction
    }

    public virtual void UpdateWaypoint(Waypoint _wp)
    {
        
    }

    
}
