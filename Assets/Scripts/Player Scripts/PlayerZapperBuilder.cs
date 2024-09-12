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
        Vector2 nearestPathPoint = GetNearestPathPoint();
        Vector2 pathDirection = GetPathDirection(nearestPath.GetComponent<BoxCollider2D>());
        Vector2 perpendicularDirection = GetPerpendicularDirection(pathDirection);
        Vector2 zapperPosition = nearestPathPoint + perpendicularDirection * zapperDistanceFromPath;
        Vector2 zapperCounterpartPosition = nearestPathPoint - (perpendicularDirection * .5f * zapperDistanceFromPath);
        Zapper zapper = Instantiate(zapperPrefab, zapperPosition, Quaternion.identity).GetComponent<Zapper>();
        zapper.CreateCounterpart(zapperCounterpartPosition);
        return zapper;

        
        //TODO: There's a special case where the player is touching a waypoint. In that situation, the zapper should bisect the angle between the two paths connected at the waypoint.

    }

    private GameObject GetNearestPath()
    {
        return player.turretDetector.GetNearestPath();
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

    private Vector2 GetPerpendicularDirection(Vector2 direction)
    {
    return new Vector2(-direction.y, direction.x);
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
