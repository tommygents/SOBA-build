using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointSpawner : Waypoint
{
    private Wave currentWave;
    private List<Wave.SpawnEvent> pendingSpawnEvents = new List<Wave.SpawnEvent>();
    private float waveStartTime;

    protected override void Start()
    {
        base.Start();
        WaveManager.OnWaveStart += HandleWaveStart;
        WaveManager.OnWaveEnd += HandleWaveEnd;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveStart -= HandleWaveStart;
        WaveManager.OnWaveEnd -= HandleWaveEnd;
    }

    private void HandleWaveStart(int waveNumber)
    {
        currentWave = WaveManager.Instance.GetWave(waveNumber);
        if (currentWave != null)
        {
            pendingSpawnEvents = new List<Wave.SpawnEvent>(currentWave.spawnEvents);
            waveStartTime = Time.time;
            StartCoroutine(SpawnRoutine());
        }
    }

    private void HandleWaveEnd()
    {
        StopAllCoroutines();
        pendingSpawnEvents.Clear();
    }

    private IEnumerator SpawnRoutine()
    {
        while (pendingSpawnEvents.Count > 0)
        {
            float currentWaveTime = Time.time - waveStartTime;
            Wave.SpawnEvent nextSpawnEvent = pendingSpawnEvents[0];

            if (currentWaveTime >= nextSpawnEvent.startTime)
            {
                pendingSpawnEvents.RemoveAt(0);
                StartCoroutine(SpawnEnemiesForEvent(nextSpawnEvent));
            }

            yield return null;
        }
    }

    private IEnumerator SpawnEnemiesForEvent(Wave.SpawnEvent spawnEvent)
    {
        float[] spawnTimes = WaveManager.Instance.GetWave(WaveManager.Instance.currentWaveNumber).GetSpawnTimesForEvent(spawnEvent);

        foreach (float spawnTime in spawnTimes)
        {
            float waitTime = spawnTime - (Time.time - waveStartTime);
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            SpawnEnemy(spawnEvent);
        }
    }

    private void SpawnEnemy(Wave.SpawnEvent spawnEvent)
    {
        Enemy enemy = Instantiate(spawnEvent.enemyPrefab, transform.position, Quaternion.identity);
        enemy.movementScript = enemy.GetComponentInChildren<EnemyWaypointMovement>();
        enemy.InitializeMovement(this, GetNextWaypoint());

        if (Random.value < spawnEvent.armorProbability)
        {
            enemy.MakeArmored();
        }
    }
}