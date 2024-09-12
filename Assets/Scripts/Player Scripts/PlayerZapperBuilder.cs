using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZapperBuilder : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject zapperPrefab;
    [SerializeField] private GameObject zapperCounterpartPrefab;
    [SerializeField] private float zapperDistanceFromPath = 2f;


    void Awake()
    {
        player = GetComponent<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        public Zapper DeployZapper()
    {
        GameObject nearestPath = GetNearestPath();
        BoxCollider2D pathCollider = nearestPath.GetComponent<BoxCollider2D>();
        Vector2 pathDirection = GetPathDirection(pathCollider);
        Vector2 perpendicularDirection = GetPerpendicularDirection(pathDirection);

        float pathWidth = GetPathWidth(pathCollider);
        Vector2 nearestPoint = GetNearestPathPoint();

        Vector2 zapperPosition = nearestPoint + (perpendicularDirection * pathWidth * zapperDistanceFromPath);
        Vector2 zapperCounterpartPosition = nearestPoint + (-perpendicularDirection * pathWidth * zapperDistanceFromPath);

        Zapper zapper = Instantiate(zapperPrefab, zapperPosition, Quaternion.identity).GetComponent<Zapper>();
        zapper.CreateCounterpart(zapperCounterpartPosition);
        return zapper;

        
        //TODO: There's a special case where the player is touching a waypoint. In that situation, the zapper should bisect the angle between the two paths connected at the waypoint.

    }

    private GameObject GetNearestPath()
    {
        return player.detectionRadius.GetNearestPath();
    }

    private float GetPathWidth(BoxCollider2D pathCollider)
    {
        return Math.Min(pathCollider.size.x, pathCollider.size.y);
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

    private Vector2 GetPerpendicularDirection(Vector2 pathDirection)
    {
        // Calculate both possible perpendicular directions
        Vector2 perp1 = new Vector2(-pathDirection.y, pathDirection.x);
        Vector2 perp2 = new Vector2(pathDirection.y, -pathDirection.x);

        // Get the player's position and the nearest point on the path
        Vector2 playerPosition = transform.position;
        Vector2 nearestPoint = GetNearestPathPoint();

        // Calculate the vector from the nearest point to the player
        Vector2 toPlayer = playerPosition - nearestPoint;

        // Choose the perpendicular direction that points more towards the player
        if (Vector2.Dot(perp1, toPlayer) > Vector2.Dot(perp2, toPlayer))
        {
            return perp1.normalized;
        }
        else
        {
            return perp2.normalized;
        }
    }

private Vector2 GetNearestPathPoint()
   {
    GameObject nearestPath = GetNearestPath();
    Collider2D _collider = nearestPath.GetComponent<Collider2D>();
    return _collider.ClosestPoint(transform.position);
   }



    #region Waypoint Special Case

//TODO: Get the angle between the two paths connected to the waypoint and create a zapper that bisects that angle.



    #endregion


}
