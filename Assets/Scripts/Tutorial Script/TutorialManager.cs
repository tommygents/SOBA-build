using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    /*
     * The steps of the tutorial:
     * TODO: Tell the player that enemies will come from the right
     * 
     * TODO: Identify the spot in the crevice as a good place to put
     * a turret
     * 
     * TODO: Tell the player how to get to the crevice;
     * TODO: once the player starts moving toward
     * the crevice, explain to them how to dash
     * 
     * TODO: Once the player arrives at that spot, tell them to squat down
     * If they stop squatting early, they'll lose the progress
     * And then it makes a turret
     * 
     * TODO: Once the turret has been made,
     * explain to the player that they
     * can enter the turret and run in place
     * 
     * TODO: Once the player has charged the turret enough,
     * maybe it launches
     * into the wave?
     * 
     * 
     * 
     */

    public bool tutorialEnabled = true;
    public GameObject creviceLocation; //This is the spot we encourage the player to head towards
    [SerializeField] private SpriteRenderer creviceTarget; //the animation to indicate the target spot

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
