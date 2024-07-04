using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public EnemyCounter Instance;
    private int enemyCount = 0;
    public static event Action OnEnemyCountZero;
    
    // Start is called before the first frame update

    void Awake()
    {
if (Instance == null)
    {
    Instance = this;
    DontDestroyOnLoad(gameObject);
    }
    else
    {
    Destroy(gameObject);
    }
    }


    void Start()
    {
        GameEvents.OnEnemyDestroyed += RemoveEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEnemy()
    {
        enemyCount++;
    }

    public void AddEnemies(int amount)
    {
        enemyCount += amount;

    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }

    public void RemoveEnemy()
    {
        enemyCount--;
        CheckEnemyCountZero();
    }

    public void RemoveEnemies(int amount)
    {
        enemyCount -= amount;
        CheckEnemyCountZero();
    }   

    private void CheckEnemyCountZero()
    {
        if (enemyCount <= 0)
        {
            enemyCount = 0;
             if (!WaveManager.Instance.isWaveActive)
            {
                FireEnemyCountZeroEvent();
            }
            
        }
    }

    private void FireEnemyCountZeroEvent()
    {
        OnEnemyCountZero?.Invoke();
    }

}
