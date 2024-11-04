using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : Ammo
{

    [SerializeField] protected CircleCollider2D damageCollider;
    protected Enemy[] enemiesInRange;
    protected DamageTypes damageType = DamageTypes.explosive;
    [SerializeField] private ParticleSystem explosionParticles;

    // Add radius indicator variables
    private LineRenderer radiusIndicator;
    [SerializeField] private int segments = 32;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private Color indicatorColor = new Color(1f, 0f, 0f, 0.3f);

    // Start is called before the first frame update
    protected override void Start()
    {
        if (damageCollider == null)
        {
            damageCollider = GetComponent<CircleCollider2D>();
        }
        base.Start();
        damageCollider.enabled = false;
        CreateRadiusIndicator();
    }

    public void AssignDamageCollider()
    {
        damageCollider = GetComponent<CircleCollider2D>();
    }

    private void CreateRadiusIndicator()
    {
        GameObject indicatorObj = new GameObject("RadiusIndicator");
        indicatorObj.transform.SetParent(transform);
        indicatorObj.transform.localPosition = Vector3.zero;

        radiusIndicator = indicatorObj.AddComponent<LineRenderer>();
        radiusIndicator.useWorldSpace = false;
        radiusIndicator.loop = true;
        radiusIndicator.positionCount = segments + 1;
        radiusIndicator.startWidth = lineWidth;
        radiusIndicator.endWidth = lineWidth;
        radiusIndicator.material = new Material(Shader.Find("Sprites/Default"));
        radiusIndicator.startColor = indicatorColor;
        radiusIndicator.endColor = indicatorColor;

        DrawRadius();
    }

    private void DrawRadius()
    {
        float deltaTheta = (2f * Mathf.PI) / segments;
        float theta = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = damageCollider.radius * Mathf.Cos(theta);
            float y = damageCollider.radius * Mathf.Sin(theta);
            
            Vector3 pos = new Vector3(x, y, 0);
            radiusIndicator.SetPosition(i, pos);

            theta += deltaTheta;
        }
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
            // Disable the radius indicator before explosion
            if (radiusIndicator != null)
            {
                radiusIndicator.enabled = false;
            }
            
            damageCollider.enabled = true;
            GetandHitTargets();
            ParticleSystem _ep = Instantiate(explosionParticles, transform.position, Quaternion.identity);
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
        // Clean up the material we created
        if (radiusIndicator != null && radiusIndicator.material != null)
        {
            Destroy(radiusIndicator.material);
        }
        base.OnDestroy();

    }

    public override void MakeAmmo(float _range, float _speed, float _angle, float _damageMultiplier)
    {
        damageCollider.radius *= _damageMultiplier;
        MakeAmmo(_range, _speed, _angle);
    }

}
