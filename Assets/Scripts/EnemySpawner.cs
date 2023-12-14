using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Enemy
{

    public override int MoveSpeed { get => base.MoveSpeed; set => base.MoveSpeed = 0; }
    public override int MaxHP { get => base.MaxHP; set => base.MaxHP = value; }
    public Enemy enemyPrefab;
    public float spawnTimer; //Tells the coroutine how often to spawn
    public float spawnDistance; //the radius of the circle in which the spawner will place new enemies
    new public Collider2D collider2D;
    public Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        MaxHP = 200;
        Invoke("StartSpawning", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());

    }

    public IEnumerator SpawnEnemy()
    {
        while (true)
        {
            float _sdx = Random.Range(-spawnDistance, spawnDistance);
            float _sdy = Random.Range(-spawnDistance, spawnDistance);
            if (_sdx < 0) { _sdx -= Mathf.Sqrt(2); } else { _sdx += Mathf.Sqrt(2); }
            if (_sdy < 0) { _sdy -= Mathf.Sqrt(2); } else { _sdy += Mathf.Sqrt(2); }
            Vector2 spawnPoint = new Vector2(transform.position.x + _sdx, transform.position.y + _sdy);
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity, this.transform);

            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
