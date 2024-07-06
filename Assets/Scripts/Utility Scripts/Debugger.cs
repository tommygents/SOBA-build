using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{

    [SerializeField] private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Jumping timer...");
            WaveManager.Instance.JumpTimerForDebugging();
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Filling Turret...");
            player.engagedTurret.FillMeterDebugging();
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Building turret...");
            player.FinishBuildingTurretDebug();
        }
    }
}
