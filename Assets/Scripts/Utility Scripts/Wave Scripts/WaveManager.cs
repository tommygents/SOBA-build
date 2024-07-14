using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class WaveManager : MonoBehaviour
{
public static WaveManager Instance;

    [Serializable]
    public class EnemyPrefabEntry
    {
        public string enemyType;
        public Enemy enemyPrefab;
    }

    public TextAsset waveDataCSV;
    [SerializeField] private List<EnemyPrefabEntry> enemyPrefabList = new List<EnemyPrefabEntry>();
    [HideInInspector] public Dictionary<string, Enemy> enemyPrefabs = new Dictionary<string, Enemy>();

    private Dictionary<int, Wave> waves = new Dictionary<int, Wave>();
    
    public int currentWaveNumber = 0;
    public float waveTimer = 0f;
    public float breakTimer = 0f;
    public float breakDuration = 30f; // Duration between waves
    public bool isWaveActive = false;
    public bool isGameOver = false;

    public static event Action<int> OnWaveStart;
    public static event Action OnWaveEnd;
    public static event Action OnGameOver;
    public static event Action OnEndGame;

void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeEnemyPrefabsDictionary();
            LoadWaveData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeEnemyPrefabsDictionary()
    {
        enemyPrefabs = enemyPrefabList.ToDictionary(entry => entry.enemyType, entry => entry.enemyPrefab);
    }

    void Update()
    {
        if (isGameOver) return;

        if (isWaveActive)
        {
            waveTimer -= Time.deltaTime;
            if (waveTimer <= 0f)
            {
                EndWave();
            }
        }
        else
        {
            breakTimer -= Time.deltaTime;
            if (breakTimer <= 0f)
            {
                StartNextWave();
            }
        }
    }

    private void LoadWaveData()
    {
        string[] lines = waveDataCSV.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        
        for (int i = 1; i < lines.Length; i++) // Skip header row
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue; // Skip blank lines

            string[] values = line.Split(',');
            if (values.Length < 9)
            {
                Debug.LogWarning($"Line {i + 1} in CSV file does not have enough values. Skipping.");
                continue;
            }

            try
            {
                int waveNumber = int.Parse(values[0]);
                if (!waves.ContainsKey(waveNumber))
                {
                    waves[waveNumber] = ScriptableObject.CreateInstance<Wave>();
                    waves[waveNumber].waveNumber = waveNumber;
                }

                if (!enemyPrefabs.ContainsKey(values[2]))
                {
                    Debug.LogError($"Enemy type '{values[2]}' not found in enemyPrefabs dictionary. Skipping line {i + 1}.");
                    continue;
                }

                Wave.SpawnEvent spawnEvent = new Wave.SpawnEvent
                {
                    startTime = float.Parse(values[1]),
                    enemyPrefab = enemyPrefabs[values[2]],
                    count = int.Parse(values[3]),
                    duration = float.Parse(values[4]),
                    randomizeSpawnTimes = bool.Parse(values[5]),
                    armorProbability = float.Parse(values[6]),
                    minSpawnIntervalFactor = float.Parse(values[7]),
                    maxSpawnIntervalFactor = float.Parse(values[8])
                };

                waves[waveNumber].AddSpawnEvent(spawnEvent);
            }
            catch (FormatException e)
            {
                Debug.LogError($"Error parsing values in line {i + 1}: {e.Message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Unexpected error processing line {i + 1}: {e.Message}");
            }
        }
    }

    public Wave GetWave(int waveNumber)
    {
        if (waves.TryGetValue(waveNumber, out Wave wave))
        {
            return wave;
        }
        Debug.LogWarning($"Wave {waveNumber} not found.");
        return null;
    }

    public int GetTotalWaveCount()
    {
        return waves.Count;
    }

    public float GetWaveDuration(int waveNumber)
    {
        if (waves.TryGetValue(waveNumber, out Wave wave))
        {
            return wave.spawnEvents.Max(e => e.startTime + e.duration);
        }
        Debug.LogWarning($"Wave {waveNumber} not found.");
        return 0f;
    }

     public bool HasNextWave()
    {
        return waves.ContainsKey(currentWaveNumber + 1);
    }

    public void StartNextWave()
    {
        currentWaveNumber++;
        if (waves.TryGetValue(currentWaveNumber, out Wave wave))
        {
            isWaveActive = true;
            waveTimer = GetWaveDuration(currentWaveNumber);
            OnWaveStart?.Invoke(currentWaveNumber);
        }
        else
        {
            EndGame();
        }
    }

    public void EndWave()
    {
        isWaveActive = false;
        OnWaveEnd?.Invoke();
        if (HasNextWave())
        {
            breakTimer = breakDuration;
        }
        else
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        isGameOver = true;
        OnGameOver?.Invoke();
        OnEndGame?.Invoke(); // Invoke the new event
    }
    public float GetRemainingWaveTime()
    {
        return waveTimer;
    }

    public float GetRemainingBreakTime()
    {
        return breakTimer;
    }

        public void JumpTimerForDebugging()
{
    if (isWaveActive)
    {
        // If we're in a wave, jump to the end of the wave
        waveTimer = 0.1f; // Set to a small value to trigger wave end on next update
    }
    else
    {
        // If we're in a break, jump to the end of the break
        breakTimer = 0.1f; // Set to a small value to trigger next wave start on next update
    }
}

}