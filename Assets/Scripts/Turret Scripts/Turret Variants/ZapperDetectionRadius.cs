using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapperDetectionRadius : MonoBehaviour
{
    [SerializeField] public List<GameObject> pathInRange;
    [SerializeField] private CircleCollider2D circleCollider;
    

    
    // Start is called before the first frame update
    protected void Start()
    {
    circleCollider = GetComponent<CircleCollider2D>();
    }

   public Vector2 GetNearestPathPoint()
   {
    GameObject nearestPath = GetNearestPath();
    Collider2D _collider = nearestPath.GetComponent<Collider2D>();
    return _collider.ClosestPoint(transform.position);
   }
    
    public GameObject GetNearestPath()
    {
        GameObject nearestPath = null;
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject path in pathInRange)
        {
            Collider2D _collider = path.GetComponent<Collider2D>();
            float distance = Vector2.Distance(transform.position, _collider.ClosestPoint(transform.position));
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPath = path;
            }
        }
        Debug.Log("Nearest path: " + nearestPath);
        return nearestPath;
    }

    public float GetPathWidth()
    {
        GameObject nearestPath = GetNearestPath();
        BoxCollider2D _collider = nearestPath.GetComponent<BoxCollider2D>();
        return _collider.size.x < _collider.size.y ? _collider.size.x : _collider.size.y;
        
    }

    protected void GetAllPathsInRange()
    {
         Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,circleCollider.radius);
    Debug.Log("Colliders in range: " + colliders.Length);
    // Filter the colliders to get only the Path objects
    foreach (Collider2D collider in colliders)
    {
        Debug.Log("Collider: " + collider.gameObject.layer);
        if (collider.gameObject.layer == LayerMask.NameToLayer("PathCollider"))
        {
            pathInRange.Add(collider.gameObject);
        }
    }
    Debug.Log("Paths in range: " + pathInRange.Count);
    }




public void Initialize()
{
 pathInRange = new List<GameObject>();
    GetAllPathsInRange();
}

    public float GetTargetingRadius()
    {
        if (circleCollider != null)
            return circleCollider.radius;

        else return 0f;
    }
}
