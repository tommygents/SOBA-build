using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{

    private Enemy parentEnemy;
    [SerializeField] public PlayerBase defaultTarget;

    void Start()
    {
        parentEnemy = GetComponentInParent<Enemy>();
        if (parentEnemy == null)
        {
            Debug.LogError("Parent Enemy script not found!");
        }
        defaultTarget = FindAnyObjectByType<PlayerBase>();
        parentEnemy.movementScript.SetTarget(defaultTarget.gameObject);
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Player _player = other.GetComponent<Player>();
        if(_player != null )
        {
            parentEnemy.movementScript.SetTarget(other.gameObject);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        Player _player = other.GetComponent<Player>();
        if (_player != null)
        {
            parentEnemy.movementScript.SetTarget(defaultTarget.gameObject);
        }
    }
}
