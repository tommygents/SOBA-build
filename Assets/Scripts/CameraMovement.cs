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
            Vector3 direction = Vector3.zero; // Initialize direction as zero

            // Check if the player is outside the deadzone on the x-axis
            if (playerViewportPos.x > 0.5f + deadzone.x || playerViewportPos.x < 0.5f - deadzone.x)
            {
                direction.x = player.position.x - transform.position.x; // Accumulate x-axis difference
            }

            // Check if the player is outside the deadzone on the y-axis
            if (playerViewportPos.y > 0.5f + deadzone.y || playerViewportPos.y < 0.5f - deadzone.y)
            {
                direction.y = player.position.y - transform.position.y; // Accumulate y-axis difference
            }

            if (direction != Vector3.zero) // Check if there is any movement needed
            {
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);
            }
        
        /*
        // Check if the player is outside the deadzone
        if (playerViewportPos.x > 0.5f + deadzone.x || playerViewportPos.x < 0.5f - deadzone.x ||
            playerViewportPos.y > 0.5f + deadzone.y || playerViewportPos.y < 0.5f - deadzone.y)
        {
            // Calculate the direction vector in world space
            
           
            Vector3 direction = player.position - cameraCenter;
            direction.z = 0; // Assuming a 2D plane movement

            // Translate the camera
            
        }
        */

    }
}
