using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealPull : PlayerAttack
{

    public override int Damage
    {
        get { return base.Damage; }
        set { base.Damage = value; } // You can add additional logic here
    }
    public float maxDuration = 1f; //the duration, in seconds, of an attack
    public float duration = 0f;
    //public CircleCollider2D circleCollider;
    public float maxRadius = 0.25f; //this is the final radius of the attack
    public SpriteRenderer spriteRenderer;
    public bool attackStarted = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
       
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = maxRadius * 2f * Vector3.one;
        attackStarted = true;
        Debug.Log("Heal Started");

    }

    // Update is called once per frame
    protected override void Update()
    {
        if (attackStarted)
        {
            duration += Time.deltaTime;
            Grow();
            if (duration >= maxDuration)
            {
                Destroy(this.gameObject);
            }

        }
    }

    public void Grow()
    {
        float newRadius = Mathf.Lerp(maxRadius, 0f, duration / maxDuration);
       
        transform.localScale = Vector3.one * newRadius * 2f;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
