using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Zapper : Turret
{
    [SerializeField] private ZapperDetectionRadius zapperDetectionRadius;
    [SerializeField] private GameObject counterpartPrefab;
    [SerializeField] private ZapperCounterpart zapperCounterpart;
    private ZapperAmmo zapperAmmo;
    [SerializeField] private float targetUninteractedRunTime; //This is how long I want the turret to be able to run without interacting at all with anything else
    bool zapperActive = true;
    [SerializeField] private float movementPenalty = 0.5f;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        zapperDetectionRadius = GetComponentInChildren<ZapperDetectionRadius>();
        zapperDetectionRadius.Initialize();
        CreateCounterpart();
        InstantiateZapperAmmo();
        EnemyCounter.OnEnemyCountZero += HandleLastEnemy;
        WaveManager.OnWaveStart += HandleWaveStart;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (zapperActive)
        {
            Shoot();
            if (!HasCharge())
            {
                DeactivateZapperAmmo();
            }
        }

    }

public override void Shoot()
{
    
    DecrementChargeBar(GetAmmoCost());
    turretUI.UpdateChargeBar(ChargeBarFullPercentage(), chargeCount);
}

public void Surge(float _powerUsage)
{
    DecrementChargeBar(_powerUsage * GetAmmoCost());
    turretUI.UpdateChargeBar(ChargeBarFullPercentage(), chargeCount);
}

 protected override float GetAmmoCost()
 {
    return MinimumChargeReduction();
 }


    protected override bool ReadyToShoot()
    {
        return HasCharge() && WaveManager.Instance.isWaveActive;
    }


    public override float GetTargetingRadius()
    {
        return zapperDetectionRadius.GetTargetingRadius();
    }


   private void CreateCounterpart()
{
    GameObject _nearestPath = zapperDetectionRadius.GetNearestPath();
    if (_nearestPath != null)
    {
        // Get the collider of the nearest path
        BoxCollider2D pathCollider = _nearestPath.GetComponent<BoxCollider2D>();

        // Calculate the closest point on the path's collider to the current object
        Vector2 closestPointOnPath = pathCollider.ClosestPoint(transform.position);

        // Get the direction of the path at the closest point
        Vector2 pathDirection = GetPathDirection(pathCollider);

        // Calculate the perpendicular direction to the path
        Vector2 perpendicularDirection = new Vector2(-pathDirection.y, pathDirection.x).normalized;

        // Calculate the position to instantiate the counterpart prefab
        float offsetDistance = 2f; // Adjust this value to set the distance of the counterpart from the path
        Vector2 counterpartPosition = closestPointOnPath + perpendicularDirection * offsetDistance;

        // Instantiate the counterpart prefab at the calculated position
        zapperCounterpart = Instantiate(counterpartPrefab, counterpartPosition, Quaternion.identity, transform).GetComponent<ZapperCounterpart>();
    }
}

private Vector2 GetPathDirection(BoxCollider2D pathCollider)
{
    // Get the rotation angle of the box collider in degrees
    float rotationAngle = pathCollider.transform.eulerAngles.z;

    // Convert the rotation angle from degrees to radians
    float rotationRadians = rotationAngle * Mathf.Deg2Rad;

    // Calculate the direction vector based on the rotation angle
    Vector2 direction = new Vector2(Mathf.Cos(rotationRadians), Mathf.Sin(rotationRadians));

    return direction.normalized;
}

#region instantiating zapper ammo
public void InstantiateZapperAmmo()
{
    zapperAmmo = Instantiate(ammunition, transform.position, Quaternion.identity, transform).GetComponent<ZapperAmmo>();
    zapperAmmo.transform.position = GetMidpointWithCounterpart();
    zapperAmmo.transform.rotation = GetAngleToCounterpart();
    zapperAmmo.transform.localScale = new Vector3(.1f, GetDirectionToCounterpart().magnitude, 1f);
    zapperAmmo.SetZapper(this);
    zapperAmmo.SetMovementPenalty(movementPenalty);

}

private Vector2 GetMidpointWithCounterpart() {

    return (transform.position + zapperCounterpart.transform.position) / 2f;
}

private Vector2 GetDirectionToCounterpart() {
    return zapperCounterpart.transform.position - transform.position;
}

private Quaternion GetAngleToCounterpart() {
    return Quaternion.Euler(0, 0, Mathf.Atan2(GetDirectionToCounterpart().y, GetDirectionToCounterpart().x));
}

private void ActivateZapperAmmo() {
    zapperAmmo.gameObject.SetActive(true);
    zapperActive = true;
}

private void DeactivateZapperAmmo() {
    zapperAmmo.gameObject.SetActive(false);
    zapperActive = false;
}
#endregion

private void HandleLastEnemy()
{
    DeactivateZapperAmmo();
}


private void HandleWaveStart(int _i)
{
    
    ActivateZapperAmmo();
}

private float MinimumChargeReduction()
{
    return (Time.deltaTime * maxCharge) / targetUninteractedRunTime;
}

}
