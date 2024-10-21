using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public bool IsNearTurret { get; private set; }
    public bool IsEngagedWithTurret { get; private set; }
    public Turret NearbyTurret { get; private set; }
    public Turret EngagedTurret { get; private set; }

    private InputManager inputManager;

    public void Initialize()
    {
        inputManager = InputManager.Instance;
        inputManager.OnInteract += HandleInteract;
    }

    private void HandleInteract()
    {
        if (IsNearTurret && !IsEngagedWithTurret)
        {
            EnterTurret();
        }
        else if (IsEngagedWithTurret)
        {
            ExitTurret();
        }
    }

    private void EnterTurret()
    {
        if (NearbyTurret != null)
        {
            IsEngagedWithTurret = true;
            EngagedTurret = NearbyTurret;
            // Additional logic for entering turret
        }
    }

    private void ExitTurret()
    {
        IsEngagedWithTurret = false;
        EngagedTurret = null;
        // Additional logic for exiting turret
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Turret>() != null)
        {
            IsNearTurret = true;
            NearbyTurret = collision.GetComponent<Turret>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Turret>() != null)
        {
            IsNearTurret = false;
            NearbyTurret = null;
        }
    }

    private void OnDisable()
    {
        inputManager.OnInteract -= HandleInteract;
    }
}
