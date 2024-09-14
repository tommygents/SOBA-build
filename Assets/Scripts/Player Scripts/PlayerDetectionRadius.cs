using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerDetectionRadius : MonoBehaviour
{
    public bool detectsTurret = false;
    public Turret detectedTurret= null;
    public bool detectsPath = false;
    public Turret nearestTurret = null;
    public List<Turret> detectedTurrets = new List<Turret>();
    public List<GameObject> detectedPaths = new List<GameObject>();
    public List<Waypoint> detectedWaypoints = new List<Waypoint>();
    [SerializeField] private Player player;

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
            detectedPaths.Add(collision.gameObject);
            TurretSelectionUI.Instance.SwitchtoZapper();
            player.UpdateTurretSelection();
        }

        if (collision.GetComponent<Waypoint>() != null)
        {
            detectedWaypoints.Add(collision.GetComponent<Waypoint>());
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
            
            detectedPaths.Remove(collision.gameObject);
            detectsPath = detectedPaths.Count > 0;
            if (!detectsPath)
            {
                TurretSelectionUI.Instance.SwitchtoTurret();
                player.UpdateTurretSelection();
            }
        }

        if (collision.GetComponent<Waypoint>() != null)
        {
            detectedWaypoints.Remove(collision.GetComponent<Waypoint>());
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


    public GameObject GetNearestPath()
    {
        
        if (detectedPaths.Count == 0)
        {
            return null;
        }
        else if (detectedPaths.Count == 1)
        {
            return detectedPaths[0];
        }
        else
        {
            GameObject nearestPath = detectedPaths[0];
            foreach (GameObject path in detectedPaths)
            {
                if (Vector3.Distance(transform.position, path.transform.position) < Vector3.Distance(transform.position, nearestPath.transform.position))
                {
                    nearestPath = path;
                }
            }
            return nearestPath;
        }
        
    
    
    }

    public Waypoint GetNearestWaypoint()
    {
        if (detectedWaypoints.Count == 0)
        {
            return null;
        }
        else if (detectedWaypoints.Count == 1)
        {
            return detectedWaypoints[0];    
        }
        else
        {
            Waypoint nearestWaypoint = detectedWaypoints[0];
            foreach (Waypoint waypoint in detectedWaypoints)
            {
                if (Vector3.Distance(transform.position, waypoint.transform.position) < Vector3.Distance(transform.position, nearestWaypoint.transform.position))
                {
                    nearestWaypoint = waypoint;
                }
            }
            return nearestWaypoint;
        }
    }

}
