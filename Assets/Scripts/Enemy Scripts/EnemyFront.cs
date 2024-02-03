using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFront : MonoBehaviour
{
    private Enemy parentEnemy;

    void Start()
    {
        parentEnemy = GetComponentInParent<Enemy>();
        if (parentEnemy == null)
        {
            Debug.LogError("Parent Enemy script not found!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        parentEnemy.TargetEnter(other.gameObject);
        //parentEnemy.HandleCollision(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        parentEnemy.TargetExit(other.gameObject);
    }
}

