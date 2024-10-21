using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerBuildingSystem playerBuildingSystem;
    [SerializeField] private PlayerChargeSystem playerChargeSystem;
    [SerializeField] private HealthManager healthManager;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerBuildingSystem = GetComponent<PlayerBuildingSystem>();
        playerChargeSystem = GetComponent<PlayerChargeSystem>();
        healthManager = GetComponent<HealthManager>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        // Main update loop - components handle their own updates
    }

    private void Initialize()
    {
        // Initialize components if needed
        playerMovement.Initialize();
        playerInteraction.Initialize();
        playerBuildingSystem.Initialize();
        playerChargeSystem.Initialize();
    }
}