using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action<Vector2> OnMove;
    public event Action OnDash;
    public event Action OnInteract;
    public event Action OnBuild;
    public event Action<bool> OnCharge;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        // Movement
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movement != Vector2.zero)
        {
            OnMove?.Invoke(movement.normalized);
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDash?.Invoke();
        }

        // Interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteract?.Invoke();
        }

        // Build
        if (Input.GetKeyDown(KeyCode.B))
        {
            OnBuild?.Invoke();
        }

        // Charge
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnCharge?.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            OnCharge?.Invoke(false);
        }
    }
}
