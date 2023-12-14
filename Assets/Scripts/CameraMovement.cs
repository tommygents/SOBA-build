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
        Vector3 playerViewportPos = cam.WorldToViewportPoint(player.position); //gets the player's position in the camera
        //Vector3 cameraCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, playerViewportPos.z));//finds the worldPos of the center of the camera
        Vector3 cameraCenter = this.transform.position;
        Vector3 direction = cameraCenter;

        if (playerViewportPos.x > 0.5f + deadzone.x || playerViewportPos.x < 0.5f - deadzone.x) // check if the player is outside the deadzone on the x-axis
        {
            //set the camera's new position nearer the player on the x-axis
            direction.x = player.position.x - cameraCenter.x; 

        }

        if (playerViewportPos.y > 0.5f + deadzone.y || playerViewportPos.y < 0.5f - deadzone.y) //check on y-axis
        {
            direction.y = player.position.y - cameraCenter.y;
        }
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World); //move the camera according the the composite direction above

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
