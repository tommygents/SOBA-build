using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public float maxDuration = .2f; //the duration, in seconds, of an attack
    public float duration = 0f;
    public CircleCollider2D circleCollider;
    public float maxRadius = 0.5f; //this is the final radius of the attack
    public SpriteRenderer spriteRenderer;
    public bool attackStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;
        attackStarted= true;
    }

    // Update is called once per frame
    void Update()
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
        float newRadius = Mathf.Lerp(0f, maxRadius, duration / maxDuration);
        transform.localScale = Vector3.one * newRadius * 2f;
    }

}
