using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretImage : MonoBehaviour
{
    // Start is called before the first frame update

    public Turret turretPrefab;
    public SpriteRenderer spriteRenderer;
    public string turretName;
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
       // spriteRenderer.sprite = turretPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Turret GetTurretPrefab()
    {
        return turretPrefab;
    }
}
