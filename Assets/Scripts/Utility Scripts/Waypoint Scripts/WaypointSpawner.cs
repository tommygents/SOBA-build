using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class WaypointSpawner : Waypoint

{

    public float spawnInterval; //The length between units being spawned
    public float spawnFactor; // each wave gets faster
    public int waveSize; //the basic number of units spawned
    public float waveFactor; //the multiplier by which each wave gets larger 

    public Enemy[] enemies; //the array of enemies to spawn from
    public int enemyIndex = 0; //the index of the enemy to spawn
    private int waveNum;
    public float bigSpawnInterval;
    public float smallSpawnInterval;
    public int maxSmallWaveSize = 4;
    public float armorProbability = 0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        WaveManager.OnWaveStart += HandleWaveStart;
        WaveManager.OnWaveEnd += HandleWaveEnd;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleWaveStart(int _waveNumber)
    {
        
        StartCoroutine(Spawn());
        if (_waveNumber > 1)
        {
            StartCoroutine(BigSpawner());
        }
        waveNum = _waveNumber;
    }

    private void HandleWaveEnd()
    {
        //TODO: This is the function that ends a wave
        StopAllCoroutines();
        spawnInterval /= spawnFactor;
        bigSpawnInterval /= (2 * spawnFactor);//at the end of each wave, things get faster
        maxSmallWaveSize++;
        armorProbability += .1f;

    }

    
    public IEnumerator Spawn()
    {
        while (true)
        {
            Enemy _en = Instantiate(enemies[enemyIndex], this.transform.position, Quaternion.identity);
            _en.movementScript = _en.GetComponentInChildren<EnemyWaypointMovement>();
            _en.MoveSpeed = (int)(_en.MoveSpeed * (Mathf.Pow( spawnFactor,waveNum)));
            _en.InitializeMovement(this, GetNextWaypoint());
            if (Random.Range(0f, 1f) < armorProbability)
            {
                _en.MakeArmored();
            }
            yield return new WaitForSeconds(Random.Range(spawnInterval * .8f, spawnInterval * 1.2f)); 
        }
    }

    public IEnumerator BigSpawner()
    {
        while (true)
        {
            Enemy _en = Instantiate(enemies[enemyIndex+1], this.transform.position, Quaternion.identity);
            _en.movementScript = _en.GetComponentInChildren<EnemyWaypointMovement>();
            _en.MoveSpeed = (int)(_en.MoveSpeed * (spawnFactor * waveNum));
            _en.InitializeMovement(this, GetNextWaypoint());
            if (Random.Range(0f, 1f) < armorProbability)
            {
                _en.MakeArmored();
            }
            yield return new WaitForSeconds(Random.Range(bigSpawnInterval * .8f, bigSpawnInterval * 1.2f));
        }
    }

    public IEnumerator SmallSpawner()
    {
        while (true)
        {
            int _packSize = Random.Range(1, maxSmallWaveSize);
            while (_packSize > 0)
            {
                
            
            Enemy _en = Instantiate(enemies[enemyIndex+2], this.transform.position, Quaternion.identity);
            _en.movementScript = _en.GetComponentInChildren<EnemyWaypointMovement>();
            _en.MoveSpeed = (int)(_en.MoveSpeed * (spawnFactor * waveNum));
            _en.InitializeMovement(this, GetNextWaypoint());
            if (Random.Range(0f, 1f) < armorProbability)
            {
                _en.MakeArmored();
            }
            _packSize--;
            yield return new WaitForSeconds(Random.Range(.1f, .3f));
            }
            yield return new WaitForSeconds(Random.Range(smallSpawnInterval * .8f, smallSpawnInterval * 1.2f));
        }
    }
    //
    //TODO: begin spawning units
    /*
     * Needs a list of units that it's going to spawn
     * On the beginning of a wave, it starts spawning them at the 
     * appropriate intervals.
     * 
     * It's also a waypoint.
     * 
     * There's a number of units to spawn; not sure how to calculate that. Right now, I guess it just spawns as long as the wave... is
     * Ther
     * 
     */


}
