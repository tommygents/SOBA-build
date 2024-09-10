using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEntryIndicator : MonoBehaviour
{
    public object RectTransform { get; internal set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToTurret(Turret _turret)
    {
        this.transform.position = _turret.transform.position;
    }
}
