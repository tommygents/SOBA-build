using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : Ammo
{

    [SerializeField] protected CircleCollider2D damageCollider;
    protected Enemy[] enemiesInRange;
    protected DamageTypes damageType = DamageTypes.explosive;
    [SerializeField] private ParticleSystem explosionParticles;

    // Start is called before the first frame update
    protected override void Start()
    {
        damageCollider = GetComponent<CircleCollider2D>();
        base.Start();
        damageCollider.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void MakeAmmo(Vector3 _target, float _speed, float _angle)
    {
        speed = _speed;
        range = Vector3.Distance(transform.position, _target);

        Vector2 direction = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));
        direction.Normalize();

        // Set velocity
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    public override void CheckDistance()
    {
        if (Vector2.Distance(transform.position, startingPosition) > range)
        {
            damageCollider.enabled = true;
            GetandHitTargets();
            ParticleSystem _ep = Instantiate(explosionParticles, this.transform.position, Quaternion.identity);
            _ep.Play();
            Destroy(gameObject);
        }

    }

    public virtual void GetandHitTargets()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, damageCollider.radius);
        Debug.Log(hits.Length);
        foreach (Collider2D hit in hits)
        {
            float _distance = Vector2.Distance(hit.transform.position, transform.position);
            float _normalizedDistance = _distance / damageCollider.radius;
            if (hit.GetComponent<Enemy>() != null)
            {
                DealDamage(hit.GetComponent<Enemy>());
            }
        }
        
    }

    public override void DealDamage(Enemy _enemy)
    {
        _enemy.TakeIncrementalDamage(Damage, damageType);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

    }

}
