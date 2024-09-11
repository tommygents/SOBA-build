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


    [SerializeField] private PlayerBuildingPlacementUI placementUI;



    // Start is called before the first frame update
    void Start()
    {
        placementUI = GetComponent<PlayerBuildingPlacementUI>();
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
        placementUI.UpdateChargeBar(0f);
       

    }

    public bool IterateBuildCounter(float _time)
    {
        turretBuildCounter += _time; //add the new time
        
        placementUI.UpdateChargeBar(turretBuildCounter / turretBuildCounterMax); //Then, update the UI
        
        if (turretBuildCounter >= turretBuildCounterMax)
        {

           
            return true;

        }
        else return false;
    }


}
