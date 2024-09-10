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
    [SerializeField] public PlayerTurretDetector turretDetector;
    [SerializeField] private PlayerBuildingPlacement buildingPlacement;

    public float dashTimer = 0f;
    public float dashDuration = .25f;
    public float dashSpeed = 3f;
    public bool isDashing = false;

    [SerializeField, HideInInspector] public Vector3 positionBeforeDeactivation;

    public bool isMakingUISelection = false;
    
    [SerializeField] public PlayerTurretUI playerTurretUI;
    public Turret turretToBuild;

    public GameManager gameManager;

    [SerializeField] private GameObject radiusCircle;
    public AudioClip construction;
    public bool buildingStarted = false;
    

//Building placement variables

    // Start is called before the first frame update
    void Start()
    {

Initialize();


        

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckRunningAndPassCharge();

        if (isSquating && !isEngagedWithTurret) //activate the build timer, so that the player builds a turret
        {
            if (!turretDetector.CanBuild())
            {
                UpdateText(squatText, "Too close!");

            }
            if (turretDetector.CanBuild())
            {
                
                if (!buildingStarted)
                {
                    buildingStarted = true;
                    AudioManager.Instance.PlayerClip(construction);
                }

                UpdateText(squatText, "Building...");


                float _chargeTime = Time.deltaTime;
                ShowRadius(turretToBuild.GetTargetingRadius());
                if (buildingPlacement.IterateBuildCounter(_chargeTime)) //passes the charge time to the building manager, which returns true if enough time to build a turret has passed
                {
                    FinishBuildingTurret();
                }
            }

        }

        AdvanceDashTimerandCheckForDash();

        


        if (Input.GetKeyDown(KeyCode.R))  // Reloads the current scene
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (turretDetector.detectsTurret && engagedTurret == null)
        {
            UpdateText(pushText, "Enter");
        }
        else if (!isEngagedWithTurret)
        {
            UpdateText(pushText, "Dash");
        }

        
    } //END OF UPDATE FUNCTION
    
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

    private void FinishBuildingTurret()
    {
        Turret _turret = Instantiate(turretToBuild, transform.position, Quaternion.identity);
        buildingPlacement.ResetBuildCounter();
        HideRadius();
        UpdateText(squatText, "Build");
        buildingStarted = false;
        AudioManager.Instance.StopClip(construction);
        EnterTurret(_turret);
    }

    public void FinishBuildingTurretDebug()
    {
        
        FinishBuildingTurret();
    }




 
    

    private void TriggerDash()
    {
        isDashing = true;
        actualSpeed *= dashSpeed;
    }

    private bool InTurretEntryProximity()
    {
        return turretDetector.detectsTurret && engagedTurret == null;
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
        Camera.main.transform.position = new Vector3(_turret.transform.position.x, _turret.transform.position.y, Camera.main.transform.position.z); // Center camera on turret
        UpdateText(pushText, "Exit");

    }


    public void ExitTurret(Turret _turret)
    {
        Debug.Log("Exiting turret from ExitTurret function");
        isNearTurret = true;
        isEngagedWithTurret = false;
        nearbyTurret = _turret;
        engagedTurret = null; 
        this.GetComponent<SpriteRenderer>().enabled = true; // Show player sprite
        this.transform.position = positionBeforeEnteringTurret;
        positionBeforeEnteringTurret = new Vector3();// Adjust position as needed
    }


    public bool turretSelectionActive = false;
    public float turretSelectionTimer = 0f;
    public float turretSelectionTimeOut = 1.5f;

 

    public void UpdateTurretSelection()
    {
        turretToBuild = TurretSelectionUI.Instance.UpdateSelection();
        

    }
   

    public void HidePlayerDuringWave(int waveNumber)
    {
        
        if (isEngagedWithTurret)
        {
            Debug.Log("Hiding player during wave, exiting wave");
            ExitTurret(engagedTurret);
            
        }
        positionBeforeEnteringTurret = transform.position;
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

    public void ShowRadius(float _rad)
    {
        radiusCircle.transform.localScale = Vector3.one * _rad;

    }

    public void HideRadius()
    {
        radiusCircle.transform.localScale = Vector3.zero;
    }

#region UI text in lower right corner

    public TextMeshProUGUI squatText;
    public TextMeshProUGUI pullText;
    public TextMeshProUGUI pushText;
    public void UpdateText(TextMeshProUGUI _field, string _text)
    {
        _field.text = _text;
    }

#endregion
    #region Refactoring
private void Initialize()
{

  

    healthManager = GetComponent<HealthManager>();
       
        actualSpeed = baseSpeed;

        turretDetector = GetComponentInChildren<PlayerTurretDetector>();
        buildingPlacement = GetComponent<PlayerBuildingPlacement>();
        //playerTurretUI = GetComponentInChildren<PlayerTurretUI>();
        turretToBuild = playerTurretUI.MakeTurretSelection();
        HideRadius();
        UpdateText(squatText, "Build Turret");
        UpdateText(pullText, "Switch Turret Selection");
        SubscribeToInputEvents();
        UpdateLastPosition();
        
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
    !isSquating && 
    !isMakingUISelection
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
    isSquating = true;
    if (InTurretEntryProximity())
        {
            EnterTurret(turretDetector.DetectedTurret());
        }
        else if (InTurret())
        {
            Debug.Log("Exiting turret from OnSquatStart, InTurret");
            ExitTurret(engagedTurret);
        }
        else
        {
            //This is where I trigger the build process, or else check to deploy a turret
        }
}
public void OnSquatEnd(InputAction.CallbackContext _context)
{
   isSquating = false;
      

            buildingPlacement.ResetBuildCounter();
            HideRadius();
            UpdateText(squatText, "Build");
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
                UpdateTurretSelection();
}


public void OnPullStart(InputAction.CallbackContext _context)
{
    //isMakingUISelection = true;
    //playerTurretUI.ShowTowerSelectionPanel();
}

public void OnPullEnd(InputAction.CallbackContext _context)
{
    //isMakingUISelection = false;
    //playerTurretUI.HideTowerSelectionPanel();
}

#endregion


    #region Event Management
 void OnEnable()
{
    
SubscribeToInputEvents();
}


private void SubscribeToInputEvents()
{
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



