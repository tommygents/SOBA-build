using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    
    [SerializeField] private List<TurretUpgradeChargeData> allUpgradeData;
    
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

    public void ResetAllUpgrades()
    {
        foreach (var upgradeData in allUpgradeData)
        {
            upgradeData.ResetToDefault();
        }
    }

    // Call this when starting a new game or loading a scene
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetAllUpgrades();
    }
} 