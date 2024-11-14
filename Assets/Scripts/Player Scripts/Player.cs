using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

 
    
    public float baseSpeed = 1f;
    [SerializeField] private float actualSpeed;
    [SerializeField] public ControlScheme controller;
    [SerializeField] public HealthManager healthManager;


    


    public bool holdingPress = false;
    public bool holdingPull = false;

    
    public bool isRunning = false;
    public bool isSprinting = false;
    public bool isSquating = false;

    public bool isNearTurret = false;
    public bool isEngagedWithTurret = false;
    public Turret nearbyTurret = null;
    public Turret engagedTurret = null;
    public Vector3 positionBeforeEnteringTurret;
    [SerializeField] public PlayerDetectionRadius detectionRadius;
    [SerializeField] private PlayerBuildingPlacement buildingPlacement;
    [SerializeField] private PlayerZapperBuilder zapperBuilder;
 [SerializeField] private GameObject turretChargeBarSection;
//Dash variables
    public float dashTimer = 0f;
    public float dashDuration = .25f;
    public float dashSpeed = 3f;
    public bool isDashing = false;

//UI variables
    [SerializeField, HideInInspector] public Vector3 positionBeforeDeactivation;

    public bool isMakingUISelection = false;
    
    public Turret turretToBuild;

    public GameManager gameManager;

    [SerializeField] private GameObject radiusCircle;
    [SerializeField] private float newBasicTurretTargetingRadius = 0f;

//Squat variables for building

    public AudioClip construction;
    public bool buildingStarted = false;
    public bool buildingDeployable = false;

//Decay function variables   
    private float initialMultiplier = 4.0f;
    private float duration = 2.0f; // 2 seconds
    private float n = 0.5f; // Decay rate 
    private float squatDuration = 2f;
    private float pullDuration = 0f;
    private float pressDuration = 0f;

 

    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    void Start()
    {

        
    healthManager = GetComponent<HealthManager>();

    detectionRadius = GetComponentInChildren<PlayerDetectionRadius>();
  

    buildingPlacement = GetComponent<PlayerBuildingPlacement>();

    zapperBuilder = GetComponent<PlayerZapperBuilder>();


        //SubscribeToInputEvents();
        turretToBuild = TurretSelectionUI.Instance.GetTurretToBuild();
        HideRadius();
        actualSpeed = baseSpeed;
        InstructionsUIManager.Instance.squatText.SetText("Assemble", "turret");
        InstructionsUIManager.Instance.pullText.SetText("Switch", "turret selection");
        InstructionsUIManager.Instance.pushText.SetText("Dash");
        InstructionsUIManager.Instance.pullText.HideSecondaryText();
        UpdateLastPosition();
        turretChargeBarSection.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckRunningAndPassCharge();
        CheckForSquatAndAdvanceCharge();
        CheckForPullAndAdvanceCharge();
        CheckForPressAndAdvanceCharge();
        AdvanceDashTimerandCheckForDash();

        


        if (Input.GetKeyDown(KeyCode.R))  // Reloads the current scene
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }



        
    } //END OF UPDATE FUNCTION
    
    //Returns the multiplier for the squat charging after _time has passed since the beginning of the squat
    private float GetMultiplierDecay(float _time)
    {
        if (_time >= duration)
        {
            return 1.0f;
        }

        // Calculate the multiplier using the decay formula
        return 1.0f + (initialMultiplier - 1.0f) * Mathf.Pow((1.0f - _time / duration), n);
    }

  

    private void CheckForSquatAndAdvanceCharge()
    {
        if (!isSquating) return;

        squatDuration += Time.deltaTime;
        float _chargeMultiplier = GetMultiplierDecay(squatDuration);
        if (buildingPlacement.IterateBuildCounter(_chargeMultiplier * Time.deltaTime))
        {
            SetBuildingDeployable();
        }

    }

    private void CheckForPullAndAdvanceCharge()
    {
        if (!holdingPull) return;
        
        pullDuration += Time.deltaTime;
        float _chargeMultiplier = GetMultiplierDecay(pullDuration);
        float _chargeAmount = _chargeMultiplier * Time.deltaTime;
        engagedTurret.IteratePrimaryUpgradeProgressBar(_chargeAmount);
           
        

        //here, we implement the action for pull
    }

    private void CheckForPressAndAdvanceCharge()
    {
        if (!holdingPress) return;
        pressDuration += Time.deltaTime;
        float _chargeMultiplier = GetMultiplierDecay(pressDuration);
        float _chargeAmount = _chargeMultiplier * Time.deltaTime;
        engagedTurret.IterateSecondaryUpgradeProgressBar(_chargeAmount);
           
     }

    
    private void CheckRunningAndPassCharge()
        {
        if (isRunning && isEngagedWithTurret) //Passes the time spent running to an engaged turret to charge it up
        {
            engagedTurret.ChargeUp(isSprinting);
        }
    }

    private void AdvanceDashTimerandCheckForDash()
    {
        if (isDashing) //iterate the dash timer and then check if the dash has ended
        {
            dashTimer += Time.deltaTime;
            if (dashTimer > dashDuration)
            {
                actualSpeed = baseSpeed;
                dashTimer = 0f;
                isDashing = false;
            }
        }
    }

    private void SetBuildingDeployable()
    {
        buildingDeployable = true;
        AudioManager.Instance.StopClip(construction);
        SetRadius(turretToBuild.GetTargetingRadius());
        ConditionalShowRadius();
        ResetSquatText();
        //TODO: Other stuff to make building deployable
    }

    private void ConditionalShowRadius()
    {
        if (buildingDeployable)
        {
            ShowRadius();
        }
        else
        {
            HideRadius();
        }
    }

    private void DeployTurret()
    {
        Turret _turret = Instantiate(turretToBuild, transform.position, Quaternion.identity);
        EnterTurret(_turret);
        AfterDeploymentCleanup();
    }

    private void DeployZapper()
    {
        Zapper _zapper = zapperBuilder.DeployZapper();
        
        EnterTurret(_zapper);
        AfterDeploymentCleanup();
    }

    public void SwitchtoZapper()
    {
        
    }

    private void AfterDeploymentCleanup()
    {
        buildingPlacement.HideBuildCounter();
        HideRadius();
        
        
        buildingStarted = false;


        buildingDeployable = false;
        buildingPlacement.ResetBuildCounter();
    }

    public void ResetSquatText()
    {
       if (buildingDeployable) InstructionsUIManager.Instance.squatText.SetText("Deploy", "selected turret");
       else InstructionsUIManager.Instance.squatText.SetText("Assemble", "turret");
    }

    public void FinishBuildingTurretDebug()
    {
        
        DeployTurret();
    }




 
    

    private void TriggerDash()
    {
        isDashing = true;
        actualSpeed *= dashSpeed;
    }

    private bool InTurretEntryProximity()
    {
        return detectionRadius.detectsTurret && engagedTurret == null;
    }
    private bool InTurret()
    {
        return engagedTurret != null;
    }
   

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.GetComponent<Turret>() != null)
        {
            isNearTurret = true;
            nearbyTurret = collision.collider.gameObject.GetComponent<Turret>();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.GetComponent<Turret>() != null)
        {
            isNearTurret = false;
            nearbyTurret = null;
        }
    }

  

    public void EnterTurret(Turret _turret)
    {
        positionBeforeEnteringTurret = this.transform.position;
        isNearTurret = false;
        isEngagedWithTurret = true;
        nearbyTurret = null;
        engagedTurret = _turret;
        this.GetComponent<SpriteRenderer>().enabled = false; // Hide player sprite
        
        _turret.player = this;
        TurretSelectionUI.Instance.gameObject.SetActive(false);
        InstructionsUIManager.Instance.squatText.SetText("Exit", "turret");
        _turret.UpdateInstructionsTextWithUpgrades();
        TurretEntryUIManager.Instance.NormalizeInstructionLine();
        HideRadius();
        ChargeBarUIManager.Instance.gameObject.SetActive(false);
        turretChargeBarSection.SetActive(true);
        _turret.OnEntered();

    }


    public void ExitTurret(Turret _turret)
    {
        
        isNearTurret = true;
        isEngagedWithTurret = false;
        nearbyTurret = _turret;
        engagedTurret = null; 
        _turret.player = null;
        this.GetComponent<SpriteRenderer>().enabled = true; // Show player sprite
        this.transform.position = positionBeforeEnteringTurret;
        positionBeforeEnteringTurret = new Vector3();// Adjust position as needed
        ConditionalShowRadius();
        InstructionsUIManager.Instance.squatText.SetText("Enter", "turret");
        InstructionsUIManager.Instance.pullText.SetText("Switch", "turret selection");
        InstructionsUIManager.Instance.pushText.SetText("Dash", "");
        InstructionsUIManager.Instance.pushText.HideSecondaryText();
        buildingPlacement.buildingChargeBar.MakeActive();
        TurretEntryUIManager.Instance.DimInstructionLine();
        TurretSelectionUI.Instance.gameObject.SetActive(true);
        ChargeBarUIManager.Instance.gameObject.SetActive(true);
        //InGameLogger.Instance.Log("Exited turret: " + _turret.name);
        turretChargeBarSection.SetActive(false);
    }


    public bool turretSelectionActive = false;
    public float turretSelectionTimer = 0f;
    public float turretSelectionTimeOut = 1.5f;

 

    public void UpdateTurretSelection()
    {
        turretToBuild = TurretSelectionUI.Instance.UpdateSelection();
        if (turretToBuild.GetType() != typeof(Turret) || newBasicTurretTargetingRadius == 0f)
            SetRadius(turretToBuild.GetTargetingRadius());
        else SetRadius(newBasicTurretTargetingRadius);
            ConditionalShowRadius();
   
        

    }
   

    public void HidePlayerDuringWave(int waveNumber)
    {
        
        if (isEngagedWithTurret)
        {
            Debug.Log("Hiding player during wave, exiting wave");
            ExitTurret(engagedTurret);
            
        }
        positionBeforeEnteringTurret = transform.position;
        HideRadius();
        //this.gameObject.SetActive(false);  // Deactivate player GameObject
    }

    public void UpdateLastPosition()
    {
        if (!isEngagedWithTurret)
        {
            positionBeforeEnteringTurret = transform.position;
        }
        
    }

    public void ResetPositionToLastSaved()
    {
        transform.position = positionBeforeEnteringTurret;
        Debug.Log("Resetting position to last saved position");
        ConditionalShowRadius();
    }

    public Vector2 GetCameraBounds()
    {
        Camera mainCamera = Camera.main;
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;


        return new Vector2(camWidth, camHeight);
    }

    /// <summary>
    /// Enforces the camera boundson a position; returns the position, or the nearest in-bounds position.
    /// </summary>
    /// <param name="_position">The player's position.</param>
    /// <returns>The updated, in-bounds position.</returns>
    private Vector2 EnforceCameraBounds(Vector2 _position)
    {
        Vector2 camSize = GetCameraBounds();
        Camera mainCamera = Camera.main;
        Vector2 camPosition = mainCamera.transform.position;


        float minX = camPosition.x - camSize.x / 2;
        float maxX = camPosition.x + camSize.x / 2;
        float minY = camPosition.y - camSize.y / 2;
        float maxY = camPosition.y + camSize.y / 2;

        _position.x = Mathf.Clamp(_position.x, minX, maxX);
        _position.y = Mathf.Clamp(_position.y, minY, maxY);
        return _position;
    }
#region Radius Circle
private bool radiusVisible = false;
    public void SetRadius(float _rad)
    {
        radiusCircle.transform.localScale = Vector3.one * _rad;
        
    }

    private void ShowRadius()
    {
        radiusVisible = true;
        radiusCircle.SetActive(true);
    }

    public void HideRadius()
    {
        radiusCircle.SetActive(false);
        radiusVisible = false;
    }

    public void SetNewBasicTurretTargetingRadius(float _radius)
    {
        newBasicTurretTargetingRadius = _radius;
        if (turretToBuild.GetType() == typeof(Turret))
             SetRadius(newBasicTurretTargetingRadius);
    }
#endregion

    #region Refactoring
private void Initialize()
{

        healthManager = GetComponent<HealthManager>();
        detectionRadius = GetComponentInChildren<PlayerDetectionRadius>();
        buildingPlacement = GetComponent<PlayerBuildingPlacement>();
        zapperBuilder = GetComponent<PlayerZapperBuilder>();
        //SubscribeToInputEvents();
        
        
        
}

/// <summary>
/// Checks whether the player is in a state to move, then moves the player
/// </summary>
private void Move()
{
    
    if (IsMobile())
    {
        
        transform.Translate(InputManager.Instance.GetMoveVector() * actualSpeed * Time.deltaTime); 
        transform.position = EnforceCameraBounds(transform.position);  

    } 
}

private bool IsMobile() {
    return (
    !isEngagedWithTurret && 
    !isSquating
    );
}
    #endregion

#region Input Handlers


public void OnRunStart(InputAction.CallbackContext _context)
{
    isRunning = true;
}
public void OnRunEnd(InputAction.CallbackContext _context)
{
    isRunning = false;
}

public void OnSprintStart(InputAction.CallbackContext _context)
{
    OnRunStart(_context);
    isSprinting = true;
}
public void OnSprintEnd(InputAction.CallbackContext _context)
{
    OnRunEnd(_context);
    isSprinting = false;
}

public void OnSquatStart(InputAction.CallbackContext _context)
{
    //InGameLogger.Instance.Log("Squat started");
    if (InTurretEntryProximity())
        {
            Debug.Log("Entering turret from OnSquatStart, InTurretEntryProximity");
            EnterTurret(detectionRadius.DetectedTurret());
        }
        else if (InTurret())
        {
            Debug.Log("Exiting turret from OnSquatStart, InTurret");
            ExitTurret(engagedTurret);
        }
        else if (buildingDeployable)
        {
            if (turretToBuild is Zapper) DeployZapper();
            else DeployTurret();
        }
        else
        {
           isSquating = true;
           squatDuration = 0f;
        }
}
public void OnSquatEnd(InputAction.CallbackContext _context)
{
    Debug.Log("Squat ended, duration: " + squatDuration);
   isSquating = false;
      

            
            buildingStarted = false;
            AudioManager.Instance.StopClip(construction);
}

public void OnPress(InputAction.CallbackContext _context)
{
     
        if (!InTurret())
        {
            TriggerDash();
        }
}

public void OnPull(InputAction.CallbackContext _context)
{
if (!gameManager.isPaused)
    if (!InTurret())
                UpdateTurretSelection();
}


public void OnPullStart(InputAction.CallbackContext _context)
{
    if (InTurret())
    {
        engagedTurret.PrimaryUpgrade();
        holdingPull = true;
        pullDuration = 0f;
    }
}

public void OnPullEnd(InputAction.CallbackContext _context)
{
  
    holdingPull = false;
}

public void OnPressStart(InputAction.CallbackContext _context)
{
    if (InTurret())
    {
        engagedTurret.SecondaryUpgrade();
        holdingPress = true;
        pressDuration = 0f;
    }
}

public void OnPressEnd(InputAction.CallbackContext _context)
{
    holdingPress = false;
}
#endregion


    #region Event Management
 void OnEnable()
{
    InGameLogger.Instance.Log("Enabling player");
    SubscribeToInputEvents();
}


private void SubscribeToInputEvents()
{
    if (InputManager.Instance == null)
    {
        InGameLogger.Instance.LogError("InputManager is not initialized. Retrying...");
        StartCoroutine(WaitForInputManager());
        return;
    }

    InGameLogger.Instance.Log("Subscribing to input events");
    InputManager.Instance.OnSprintStart += OnSprintStart;
    InputManager.Instance.OnSprintEnd += OnSprintEnd;
    InputManager.Instance.OnSquatStart += OnSquatStart;
    InputManager.Instance.OnSquatEnd += OnSquatEnd;
    InputManager.Instance.OnRunStart += OnRunStart;
    InputManager.Instance.OnRunEnd += OnRunEnd;
    InputManager.Instance.OnPress += OnPress;
    InputManager.Instance.OnPullStart += OnPullStart;
    InputManager.Instance.OnPull += OnPull;
    InputManager.Instance.OnPullEnd += OnPullEnd;
    InputManager.Instance.OnPressStart += OnPressStart;
    InputManager.Instance.OnPressEnd += OnPressEnd;
}

private IEnumerator WaitForInputManager()
{
    while (InputManager.Instance == null)
    {
        yield return null; // Wait for the next frame
    }
    SubscribeToInputEvents(); // Subscribe once InputManager is available
}
 void OnDisable()
{
    
    InputManager.Instance.OnSprintStart -= OnSprintStart;
    InputManager.Instance.OnSprintEnd -= OnSprintEnd;
    InputManager.Instance.OnSquatStart -= OnSquatStart;
    InputManager.Instance.OnSquatEnd -= OnSquatEnd;
    InputManager.Instance.OnRunStart -= OnRunStart;
    InputManager.Instance.OnRunEnd -= OnRunEnd;
    InputManager.Instance.OnPress -= OnPress;

    InputManager.Instance.OnPullStart -= OnPullStart;
    InputManager.Instance.OnPull -= OnPull;
    InputManager.Instance.OnPullEnd -= OnPullEnd;
}
    #endregion

}




