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
            
            detectedTurrets.Add(collision.GetComponent<Turret>());
            detectedTurret = GetNearestTurret();
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
            TurretEntryUIManager.Instance.HideTurretEntryUI();
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
        TurretEntryUIManager.Instance.DisplayTurretEntryUI(nearestTurret);
    }
    }
    public Turret GetNearestTurret()
    {
        UpdateNearestTurret();
        return nearestTurret;
    }

    public void IndicateTurret()
    {
        //TODO: This is the code that tells the player that they will enter a turret.
        //TODO: here's what needs to happen:
        //TODO: somehow, the turret for this circle needs to get turned on.
        //TODO: then, that circle neets to connect, with a line, to the instructions layer.
        //TODO: finally, the instructions layer needs to update.
    }

    
}
