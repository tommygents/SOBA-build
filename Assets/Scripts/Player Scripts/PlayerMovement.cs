using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.25f;

    private Vector2 moveVector;
    private float actualSpeed;
    private bool isDashing;
    private float dashTimer;

    private InputManager inputManager;

    public void Initialize()
    {
        inputManager = InputManager.Instance;
        inputManager.OnMove += HandleMove;
        inputManager.OnDash += HandleDash;
    }

    private void Update()
    {
        Move();
        UpdateDash();
    }

    private void HandleMove(Vector2 input)
    {
        moveVector = input;
    }

    private void HandleDash()
    {
        if (!isDashing)
        {
            isDashing = true;
            dashTimer = dashDuration;
        }
    }

    private void Move()
    {
        actualSpeed = isDashing ? dashSpeed : baseSpeed;
        transform.Translate(moveVector * actualSpeed * Time.deltaTime);
    }

    private void UpdateDash()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }
    }

    private void OnDisable()
    {
        inputManager.OnMove -= HandleMove;
        inputManager.OnDash -= HandleDash;
    }
}
