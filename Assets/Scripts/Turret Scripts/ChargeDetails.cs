using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeDetails : MonoBehaviour
{
    //Decay function parameters, which will determine how quickly the charge rate falls off from the initial movement
    [SerializeField] private float initialMultiplier = 4.0f;
    [SerializeField] private float duration = 2.0f; // 2 seconds
    [SerializeField] private float decayRate = 0.5f; // Decay rate 

    //TODO: There's a variable, duration, that counts how long the player has been charging. That gets fed to the decay function.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual float DecayFunction(float _time)
    {
        if (_time >= duration)
        {
            return 1.0f;
        }

        // Calculate the multiplier using the decay formula
        return 1.0f + (initialMultiplier - 1.0f) * Mathf.Pow((1.0f - _time / duration), decayRate);
    }



}
