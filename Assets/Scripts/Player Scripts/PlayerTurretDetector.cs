using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretDetector : MonoBehaviour
{
    public bool detectsTurret = false;
    public Turret detectedTurret= null;

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
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Turret>() != null)
        {
            detectsTurret= false;
            detectedTurret= collision.GetComponent<Turret>();
        }
    }

    public Turret DetectedTurret()
    {
        return detectedTurret;
    }
}
