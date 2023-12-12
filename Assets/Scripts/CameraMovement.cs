using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player; // Reference to the player's transform
   
    
    public Vector2 deadzone = new Vector2(0.2f, 0.2f); // Size of the deadzone (x for horizontal, y for vertical)
    private Camera cam;
    
    public float moveSpeed;

    void Start()
    {

        cam = Camera.main;
        moveSpeed = player.GetComponent<Player>().speed * 1.5f;
    }

    void Update()
    {
        Vector3 playerViewportPos = cam.WorldToViewportPoint(player.position);

        // Check if the player is outside the deadzone
        if (playerViewportPos.x > 0.5f + deadzone.x || playerViewportPos.x < 0.5f - deadzone.x ||
            playerViewportPos.y > 0.5f + deadzone.y || playerViewportPos.y < 0.5f - deadzone.y)
        {
            // Calculate the direction vector in world space
            Vector3 playerWorldPos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, playerViewportPos.z));
            //Vector3 direction = playerWorldPos - transform.position;
            Vector3 direction = player.position - playerWorldPos;
            direction.z = 0; // Assuming a 2D plane movement

            // Translate the camera
            transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
