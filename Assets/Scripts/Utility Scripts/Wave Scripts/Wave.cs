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
        public float startTime;
        public Enemy enemyPrefab;
        public int count;
        public float duration;
        public bool randomizeSpawnTimes;
        public float armorProbability;
        public float minSpawnIntervalFactor;
        public float maxSpawnIntervalFactor;

        public float GetEndTime()
        {
            return startTime + duration;
        }
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
