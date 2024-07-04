using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretDetector : MonoBehaviour
{
    public bool detectsTurret = false;
    public Turret detectedTurret= null;
    public bool detectsPath = false;
    public Turret nearestTurret = null;
    public List<Turret> detectedTurrets = new List<Turret>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Turret>() != null)
        {
            detectsTurret= true;
            detectedTurret = collision.GetComponent<Turret>();
            detectedTurrets.Add(collision.GetComponent<Turret>());
        } 

        if (collision.gameObject.layer == LayerMask.NameToLayer("PathCollider"))
        {
            detectsPath = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Turret>() != null)
        {
            
            detectedTurret= collision.GetComponent<Turret>();
            detectedTurrets.Remove(collision.GetComponent<Turret>());
            detectedTurret = GetNearestTurret();
        } 
        if (collision.gameObject.layer == LayerMask.NameToLayer("PathCollider"))
        {
            detectsPath = false;
        }
    }

    public Turret DetectedTurret()
    {
        return detectedTurret;
    }

    public bool CanBuild()
    {
        return !(detectsTurret || detectsPath);
    }

private void UpdateNearestTurret()
    {
        if (detectedTurrets.Count == 0)
        {
            nearestTurret = null;
            detectsTurret = false;
        }
        else
        {
            detectsTurret = true;
        nearestTurret = detectedTurrets[0];
        foreach (Turret turret in detectedTurrets)
        {
            if (Vector3.Distance(transform.position, turret.transform.position) < Vector3.Distance(transform.position, nearestTurret.transform.position))
            {
                nearestTurret = turret;
            }
        }
    }
    }
    public Turret GetNearestTurret()
    {
        UpdateNearestTurret();
        return nearestTurret;
    }

    
}
