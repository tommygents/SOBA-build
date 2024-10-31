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
     public int displayWaveNumber = 1;
    public float waveTimer = 0f;
    public float breakTimer = 0f;
    public float breakDuration = 30f; // Duration between waves
    public bool isWaveActive = false;
    public bool isGameOver = false;

    public static event Action<int> OnWaveStart;
    public static event Action OnWaveEnd;
    public static event Action OnGameOver;
    public static event Action OnEndGame;
    public WaveManagerUI waveManagerUI;
    private float waveStartTime;
    private float breakStartTime;

    void Start()
    {
        // Ensure Time.timeScale is set to 1
        Time.timeScale = 1f;
    }

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

    void FixedUpdate()
    {
        if (isGameOver) return;

        if (isWaveActive)
        {
            float elapsedTime = Time.time - waveStartTime;
            float waveDuration = GetWaveDuration(currentWaveNumber);
            float remainingTime = Mathf.Max(0, waveDuration - elapsedTime);
            waveManagerUI.UpdateTimer(remainingTime);
            waveManagerUI.isWaveActiveUI(true);
            waveManagerUI.waveNumberUI(displayWaveNumber);  // Update UI with display wave number
            
            if (elapsedTime >= waveDuration)
            {
                //Debug.Log($"Ending Wave {currentWaveNumber} at elapsed time: {elapsedTime}");
                EndWave();
            }
        }
        else
        {
            float elapsedTime = Time.time - breakStartTime;
            float remainingTime = Mathf.Max(0, breakDuration - elapsedTime);
            waveManagerUI.UpdateTimer(remainingTime);
            waveManagerUI.isWaveActiveUI(false);
            waveManagerUI.waveNumberUI(displayWaveNumber);  // Show next wave number during break
            if (remainingTime <= 0f)
            {
                StartNextWave();
            }
        }
    }
/*
void Update()
    {
        if (isGameOver) return;

        if (isWaveActive)
        {
            waveTimer -= Time.deltaTime;
            waveManagerUI.UpdateTimer(waveTimer);
            waveManagerUI.isWaveActiveUI(true);
            if (waveTimer <= 0f)
            {
                EndWave();
            }
        }
        else
        {
            breakTimer -= Time.deltaTime;
            waveManagerUI.UpdateTimer(breakTimer);
            waveManagerUI.isWaveActiveUI(false);
            if (breakTimer <= 0f)
            {
                StartNextWave();
            }
        }
    
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
    }
*/


private void LoadWaveData()
{
    if (waveDataCSV == null)
    {
        Debug.LogError("Wave data CSV file is not assigned!");
        return;
    }

    Debug.Log("Starting to load wave data...");
    string[] lines = waveDataCSV.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
    Debug.Log($"Found {lines.Length} lines in the CSV file.");
    
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
                Debug.Log($"Created new wave: {waveNumber}");
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
            Debug.Log($"Added spawn event to wave {waveNumber}: {values[2]} x{spawnEvent.count}");
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

    Debug.Log($"Finished loading wave data. Total waves: {waves.Count}");
}    public Wave GetWave(int waveNumber)
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
            waveStartTime = Time.time;
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
            displayWaveNumber++;  // Increment display wave number at the end of the wave
            breakStartTime = Time.time;
        }
        else
        {
            EndGame();
        }
    }

public float GetWaveDuration(int waveNumber)
{
    const float MAX_WAVE_DURATION = 60f;

    if (waves.TryGetValue(waveNumber, out Wave wave))
    {
        float latestEndTime = 0f;
        foreach (var spawnEvent in wave.spawnEvents)
        {
            float endTime = Mathf.Min(spawnEvent.GetEndTime(), MAX_WAVE_DURATION);
            //Debug.Log($"Wave {waveNumber} - Event: {spawnEvent.enemyPrefab.name}, Start: {spawnEvent.startTime}, Duration: {spawnEvent.duration}, End: {endTime}");
            latestEndTime = Mathf.Max(latestEndTime, endTime);
        }
        float waveDuration = Mathf.Min(latestEndTime, MAX_WAVE_DURATION);
        Debug.Log($"Wave {waveNumber} calculated duration: {waveDuration}");
        return waveDuration;
    }
    Debug.LogWarning($"Wave {waveNumber} not found.");
    return 0f;
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
        waveStartTime = Time.time - GetWaveDuration(currentWaveNumber) + 0.1f;
    }
    else
    {
        // If we're in a break, jump to the end of the break
        breakStartTime = Time.time - breakDuration + 0.1f;
    }
}

}