using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretImage : Image
{
    // Start is called before the first frame update

    public Turret turretPrefab;
    
    
    public string turretName;

    
    

    protected override void Awake()
    {
        base.Awake();
    

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Turret GetTurretPrefab()
    {
        return turretPrefab;
    }

    public Vector2 GetRectTransformPosition()
    {
        return rectTransform.anchoredPosition;
    }
}
