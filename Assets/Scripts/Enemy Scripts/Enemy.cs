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
    [SerializeField] protected float incrementalDamage = 0f;

    [SerializeField]
    private GameObject targetGO; //The game object that the unit moves towards
    public virtual GameObject TargetGO
    {
        get { return targetGO; }
        set { targetGO = value; }
    }

    protected Waypoint nextWaypoint; //the next waypoint that the enemy is moving toward

    [SerializeField]  private int pointsValue = 5;
    public virtual int PointsValue
    {
        get { return pointsValue; }
        set { pointsValue = value; }
    }

    public Vector2 previousPosition;
    public Vector2 currentPosition;
    public Vector2 velocity;

    public SpriteRenderer spriteRenderer;
    //public bool targetInRange = false;
    public EnemyWaypointMovement movementScript;
    [SerializeField] private Player player;
    public ParticleSystem deathParticles;
    public AudioClip deathSound;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        hp = MaxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
      
        
            movementScript = GetComponentInChildren<EnemyWaypointMovement>();
       
        
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

        
        movementScript.Move();
        currentPosition = transform.position;
        velocity = (currentPosition - previousPosition) / Time.deltaTime;
        previousPosition = currentPosition;
    }

    public void PassNewWaypoint(Waypoint _wp)
    {
        movementScript.UpdateWaypoint(_wp);
    }


    public void Die()
    {
        //Called when an enemy's hp gets to zero
        ScoreThisEnemy();
        ParticleSystem _dp = Instantiate(deathParticles, this.transform.position, Quaternion.identity);
        _dp.Play();
        AudioManager.Instance.enemyAudio.clip = deathSound;
        AudioManager.Instance.enemyAudio.Play();
        
        RemoveThisEnemy();

    }

    private void ScoreThisEnemy()
    {
        GameEvents.EnemyKilled(PointsValue);
    }

    private void RemoveThisEnemy()
    {
    GameEvents.EnemyDestroyed();
    Destroy(this.gameObject);
    }



    public void TargetEnter(GameObject _target)
    {
        movementScript.TargetEnter(_target);
    }
    public void TargetExit(GameObject _target)
    {
        movementScript.TargetExit(_target);
    }

    public void InitializeMovement(WaypointSpawner _sp, Waypoint _wp)
    {
        
        if (_wp == null || _sp == null)
        {
            Debug.LogError("Initialization failed: Waypoint or WaypointSpawner is null.");
            return;
        }
        
        movementScript.nextWaypoint = _wp;
        movementScript.prevWaypoint = _sp;
        movementScript.parent = this;
        PassMovementSpeed();
    }
    public void PassMovementSpeed()
    {
        movementScript.speed = moveSpeed;
    }

    public void TakeIncrementalDamage(float _damage)
    {
        int _intDamage = (int)_damage;
        float _floatDamage = _damage - _intDamage;
        hp -= _intDamage;
        incrementalDamage += _floatDamage;
        if (incrementalDamage >= 1f)
        {
            hp--;
            incrementalDamage -= 1f;
        }

    }

    public void SetMovementPenalty(float _penalty)
    {
        movementScript.speed = moveSpeed * (1 - _penalty);
    }


    #region adding in armor
    public bool armored;
    public Material armorMaterial;
    public int armorHP;

    public void TakeIncrementalDamage(float _damage, DamageTypes _dt)
    {
        if (armored)
        {
            if (_dt == DamageTypes.electric)
            {
            _damage *= 2f;
            }
            if (_dt == DamageTypes.explosive)
            {
                _damage = Damage;
            }
            else
            {
                _damage *= .5f;
            } 

        }
        TakeIncrementalDamage(_damage);

    }


    #endregion

}
