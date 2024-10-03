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
    [SerializeField] private Sprite defaultSprite;
    //public bool targetInRange = false;
    public EnemyWaypointMovement movementScript;
    [SerializeField] private Player player;
    public ParticleSystem deathParticles;
    public AudioClip deathSound;


       protected virtual void Awake()
    {
        hp = MaxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
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
        if (armored)
        {
            TakeArmorDamage((int)_damage);
                _damage -= (int)_damage;
            
            if (armorHP <= 0)
            {
                _intDamage = -armorHP;
                armorHP = 0;
                DestroyArmor();
            }
            else _intDamage = 0;
        }
        hp -= _intDamage;
        incrementalDamage += _floatDamage;
        if (incrementalDamage >= 1f)
        {
            if (armored)
            {
                armorHP -= 1;
                if (armorHP <= 0)
                    DestroyArmor();
            }
            else hp--;
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
    public int defaultArmorHP;
    [SerializeField] private Sprite[] armorSprites;
    [SerializeField] private int currentArmorSpriteIndex;
    [SerializeField] private int armorIncrement;

    public void TakeIncrementalDamage(float _damage, DamageTypes _dt)
    {
        if (armored)
        {
            if (armorHP/GetArmorFactor(_dt) <= _damage) //Case 1: The damage will wipe out the armor entirely
            {
                _damage -= armorHP/GetArmorFactor(_dt);
                armorHP = 0;
                DestroyArmor();
            }
            else //Case 2: the damage will not wipe out the armor.
            {
                _damage *= GetArmorFactor(_dt);
                 
            }
        TakeIncrementalDamage(_damage);

        }
    }

    protected float GetArmorFactor(DamageTypes _dt)
    {
        if (_dt == DamageTypes.electric)
            return 2f;
        if (_dt == DamageTypes.explosive)
            return 1f;
        return .5f;
    }
    public void DestroyArmor()
    {
        armored = false;
        
        spriteRenderer.sprite = defaultSprite;
      
    }

    private void TakeArmorDamage(int _damage)
    {
        armorHP -= (int)_damage;
        if (armorHP <= 0)
        {
            DestroyArmor();
            return;
        }
        int _gap = (armorIncrement * currentArmorSpriteIndex) - armorHP;
        if (_gap > 0)
        {
            currentArmorSpriteIndex -= (int)(_gap/armorIncrement);
            spriteRenderer.sprite = armorSprites[currentArmorSpriteIndex];
        }

        /*while (armorHP < armorIncrement * currentArmorSpriteIndex)
        {
            currentArmorSpriteIndex--;
        }*/
        spriteRenderer.sprite = armorSprites[currentArmorSpriteIndex];
    }

    public void MakeArmored(int armorStrength)
    {
        currentArmorSpriteIndex = armorSprites.Length - 1;
        if (spriteRenderer == null)

            spriteRenderer = GetComponent<SpriteRenderer>();
        armored = true;
        armorHP = armorStrength;
        defaultArmorHP = armorStrength;
        armorIncrement = defaultArmorHP / armorSprites.Length;
        defaultSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = armorSprites[currentArmorSpriteIndex];
        
        
    }

    public void MakeArmored()
    {
        MakeArmored(defaultArmorHP);
    }


    #endregion

}
