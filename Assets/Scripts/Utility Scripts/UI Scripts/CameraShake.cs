using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float baseShakeMagnitude = 0.1f;
    public float maxShakeMagnitude = 0.5f;

    private Vector3 originalPosition;
    private float shakeTimer;
    private int shakeCount = 0;

    void Awake()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            // Calculate current shake magnitude based on shakeCount
            float currentMagnitude = Mathf.Min(baseShakeMagnitude * shakeCount, maxShakeMagnitude);
            transform.localPosition = originalPosition + Random.insideUnitSphere * currentMagnitude;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;
            shakeCount = 0; // Reset shake count when not shaking
        }
    }

    public void TriggerShake()
    {
        shakeCount++;
        shakeTimer = shakeDuration; // Reset the timer every time a new shake is triggered
    }
}