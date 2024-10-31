using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingPlacement : MonoBehaviour
{
    public Turret[] turrets;
    public int activeTurretIndex;
    public float turretBuildCounter = 0f;
    public float turretBuildCounterMax = 5.0f;
    public Turret currentTurret;
    public ChargeBar buildingChargeBar;
    [SerializeField] private PlayerBuildingPlacementUI placementUI;
    [SerializeField] private ChargeData buildingChargeData;

    // Start is called before the first frame update
    void Start()
    {
        placementUI = GetComponent<PlayerBuildingPlacementUI>();
        buildingChargeBar.MakeActive();
        StartCoroutine(WaitForChargeBarUIManager());
        ResetBuildCounter();
    }
    private IEnumerator WaitForChargeBarUIManager()
{
    while (ChargeBarUIManager.Instance == null)
    {
        yield return null; // Wait for the next frame
    }
    buildingChargeBar.MakeActive(); // Subscribe once InputManager is available
}

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void HideBuildCounter()
    {
        
        
        
    }

    public bool IterateBuildCounter(float _time)
    {
        buildingChargeData.currentCharge += _time;
        buildingChargeBar.SetChargeAmount(buildingChargeData.currentCharge / buildingChargeData.maxCharge);
        
        return buildingChargeData.IsFull();
    }

    public void ResetBuildCounter()
    {
        buildingChargeData.ResetChargeAmount();
        buildingChargeBar.ResetChargeAmount();
    }
}
