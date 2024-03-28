using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingPlacement : MonoBehaviour
{
    public Turret[] turrets;
    public int activeTurretIndex;
    public float turretBuildCounter = 0f;
    public float turretBuildCounterMax = 10.0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void ResetBuildCounter()
    {
        turretBuildCounter = 0f;
        //TODO: this will eventually also interact with the UI
    }

    public bool IterateBuildCounter(float _time)
    {
        turretBuildCounter += _time;
        if (turretBuildCounter >= turretBuildCounterMax)
        {

           
            return true;

        }
        else return false;
    }


}
