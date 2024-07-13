using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "New Wave", menuName = "Game/Wave")]
public class Wave : ScriptableObject
{
    [Serializable]
    public class SpawnEvent
    {
        public Enemy enemyPrefab;
        public float startTime;
        public int count = 1;
        public float duration = 0f;
        public bool randomizeSpawnTimes = false;
        [Range(0f, 1f)]
        public float armorProbability = 0f;
        [Range(0.5f, 1f)]
        public float minSpawnIntervalFactor = 0.8f;
        [Range(1f, 1.5f)]
        public float maxSpawnIntervalFactor = 1.2f;
    }

    public int waveNumber;
    public List<SpawnEvent> spawnEvents = new List<SpawnEvent>();

    public void AddSpawnEvent(SpawnEvent spawnEvent)
    {
        spawnEvents.Add(spawnEvent);
        spawnEvents = spawnEvents.OrderBy(e => e.startTime).ToList();
    }

 public float[] GetSpawnTimesForEvent(SpawnEvent spawnEvent)
    {
        float[] spawnTimes = new float[spawnEvent.count];
        float baseInterval = spawnEvent.duration / (spawnEvent.count - 1);

        if (spawnEvent.randomizeSpawnTimes)
        {
            float currentTime = spawnEvent.startTime;
            for (int i = 0; i < spawnEvent.count; i++)
            {
                float minInterval = baseInterval * spawnEvent.minSpawnIntervalFactor;
                float maxInterval = baseInterval * spawnEvent.maxSpawnIntervalFactor;
                float randomInterval = UnityEngine.Random.Range(minInterval, maxInterval);
                
                spawnTimes[i] = currentTime;
                currentTime += randomInterval;
            }
        }
        else
        {
            for (int i = 0; i < spawnEvent.count; i++)
            {
                spawnTimes[i] = spawnEvent.startTime + (i * baseInterval);
            }
        }

        return spawnTimes;
    }
}
