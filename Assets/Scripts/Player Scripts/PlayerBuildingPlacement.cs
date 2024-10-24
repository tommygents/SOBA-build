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



    // Start is called before the first frame update
    void Start()
    {
        placementUI = GetComponent<PlayerBuildingPlacementUI>();
        buildingChargeBar.MakeActive();
        StartCoroutine(WaitForChargeBarUIManager());
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

    public void ResetBuildCounter()
    {turretBuildCounter = 0f;
        buildingChargeBar.ResetChargeAmount();
       

    }

    public bool IterateBuildCounter(float _time)
    {
        turretBuildCounter += _time; //add the new time
        buildingChargeBar.SetChargeAmount(turretBuildCounter / turretBuildCounterMax);       
        
        if (turretBuildCounter >= turretBuildCounterMax)
        {

           
            return true;

        }
        else return false;
    }


}
