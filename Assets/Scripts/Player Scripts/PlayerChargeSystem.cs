using UnityEngine;

public class PlayerChargeSystem : MonoBehaviour
{
    [SerializeField] private float maxChargeAmount = 100f;
    [SerializeField] private float chargeRate = 10f;

    private bool isCharging;
    private float currentChargeAmount;

    private InputManager inputManager;

    public void Initialize()
    {
        inputManager = InputManager.Instance;
        inputManager.OnCharge += HandleCharge;
    }

    private void Update()
    {
        if (isCharging)
        {
            UpdateCharge();
        }
    }

    private void HandleCharge(bool startCharging)
    {
        if (startCharging)
        {
            StartCharging();
        }
        else
        {
            StopCharging();
        }
    }

    private void StartCharging()
    {
        isCharging = true;
    }

    private void StopCharging()
    {
        isCharging = false;
        currentChargeAmount = 0f;
    }

    private void UpdateCharge()
    {
        currentChargeAmount += chargeRate * Time.deltaTime;
        currentChargeAmount = Mathf.Min(currentChargeAmount, maxChargeAmount);
    }

    public void TransferCharge(Turret turret)
    {
        if (currentChargeAmount > 0)
        {
            turret.ReceiveCharge(currentChargeAmount);
            currentChargeAmount = 0f;
        }
    }

    private void OnDisable()
    {
        inputManager.OnCharge -= HandleCharge;
    }
}
