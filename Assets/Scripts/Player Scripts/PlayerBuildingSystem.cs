using UnityEngine;

public class PlayerBuildingSystem : MonoBehaviour
{
    [SerializeField] private PlayerBuildingPlacement buildingPlacement;
    public Turret TurretToBuild { get; private set; }

    private InputManager inputManager;

    public void Initialize()
    {
        inputManager = InputManager.Instance;
        inputManager.OnBuild += HandleBuild;
    }

    private void HandleBuild()
    {
        if (TurretToBuild != null)
        {
            PlaceTurret();
        }
        else
        {
            SelectTurret();
        }
    }

    private void SelectTurret()
    {
        // Logic for selecting a turret to build
        // This might involve opening a UI or cycling through options
    }

    private void PlaceTurret()
    {
        if (buildingPlacement.CanPlaceTurret())
        {
            buildingPlacement.PlaceTurret(TurretToBuild);
            TurretToBuild = null;
        }
    }

    public void CancelBuild()
    {
        TurretToBuild = null;
    }

    private void OnDisable()
    {
        inputManager.OnBuild -= HandleBuild;
    }
}
