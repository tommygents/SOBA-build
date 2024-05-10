using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class TutorialData
{
    public List<TutorialStep> tutorialSteps;
}

[System.Serializable]
public class TutorialStep
{
    public string step;
    public string text;
}

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    public TutorialCrevice creviceLocation; // This is the spot we encourage the player to head towards
    [SerializeField] private SpriteRenderer creviceTarget; // the animation to indicate the target spot

    [SerializeField] private Animation ringPull;
    [SerializeField] private Animation ringPush;
    [SerializeField] private Animation ringPitch;
    [SerializeField] private Animation ringRoll;
    [SerializeField] private Animator ringAnimator;

    public bool tutorialEnabled; // Controls whether the tutorial is active or not

    private List<TutorialStep> steps;
    private int currentStepIndex = 0;

    [SerializeField] private ControlScheme controller;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Player player;
    [SerializeField] private float playerDistance = 0f;
    [SerializeField] private float playerMinDist = 10f;
    private Vector3 playerLastLocation;

    //Listening for specific events to fire
    [SerializeField] private bool pressHappened = false;
    private bool inputReceived = false;
    private bool squatStarted = false;
    private bool pullHappened = false;
    
    void Start()
    {
        controller = new ControlScheme();

        controller.gameplay.Enable();


        tutorialEnabled = SceneLoader.Instance.TutorialEnabled;
        if (tutorialEnabled)
        {
            LoadTutorialText();
            StartCoroutine("Tutorial");
        }
        else
        {
            tutorialPanel.SetActive(false);
            creviceLocation.gameObject.SetActive(false);
        }

        
    }

    private void LoadTutorialText()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("tutorialSteps");
        if (jsonData != null)
        {
            TutorialData loadedData = JsonUtility.FromJson<TutorialData>(jsonData.text);
            steps = loadedData.tutorialSteps;
        }
        else
        {
            Debug.LogError("Failed to load tutorial steps. Check the file path and format.");
        }
    }

    private void ShowCurrentStep()
    {
        if (currentStepIndex < steps.Count)
        {
            tutorialPanel.SetActive(true);
            tutorialText.text = steps[currentStepIndex].text;
        }
        else
        {
            EndTutorial();
        }
    }

    public void NextStep()
    {
        tutorialPanel.SetActive(false);
        if (currentStepIndex < steps.Count - 1)
        {
            currentStepIndex++;
            
        }
        else
        {
            EndTutorial();
        }
    }

    public IEnumerator Tutorial()
    {
        Debug.Log("Tutorial Initialized");
        inputManager.OnPress += HandlePress;
        inputManager.OnSquatStart += HandleSquatStart;
        inputManager.OnSquatEnd += HandleSquatFinish;
        ShowCurrentStep(); //introduction
        PlayerPrefs.SetInt("Tutorial", 1);
        yield return new WaitForSeconds(8f);
        NextStep();
        ShowCurrentStep(); //movement1
        yield return StartCoroutine(creviceLocation.BlinkSprite(10, 4f));
        
        
        yield return new WaitForSeconds(.2f);
        
        
        while (playerDistance < playerMinDist)
        {
           float _pd = Vector3.Distance(playerLastLocation, player.transform.position);
            Debug.Log($"Distance this frame: {_pd}");
            playerDistance += _pd;
            playerLastLocation = player.transform.position;
            yield return new WaitForSeconds(.01f);
        } //Wait until the player has moved... a distance, then trigger other stuff
        NextStep();
        ShowCurrentStep(); //movement3, which is about dashing
        DisplayPressAnimation();
        yield return new WaitUntil(() => pressHappened);
        HidePressAnimation();
        pressHappened = false;
        NextStep(); //cueing up waiting to get player near squat. Also need to tell player where to go.
        //Because a press happened, the player has dashed.
        //So now they're headed towards the X
        yield return new WaitUntil(() => creviceLocation.hasTouchedPlayer);
        creviceLocation.gameObject.SetActive(false);
        //Now the playerhas touched the crevice
        ShowCurrentStep(); //turret1
        DisplaySquatAnimation();
        yield return new WaitUntil(() => squatStarted);
        HideSquatAnimation();
        NextStep();
        yield return new WaitUntil(() => player.turretDetector.detectsTurret);
        Turret _turret = player.turretDetector.detectedTurret;
        ShowCurrentStep(); //turret2
        
        yield return new WaitUntil(() => player.isEngagedWithTurret);
        NextStep();
        
        ShowCurrentStep(); //turret3
        yield return new WaitUntil(() => _turret.chargeCount > 0);
        NextStep();
        ShowCurrentStep(); //turret4
        yield return new WaitUntil(() => player.isSprinting);
        yield return new WaitUntil(() => _turret.chargeCount > 1);
        NextStep();
        ShowCurrentStep(); //secondturret1
        yield return new WaitUntil(() => !player.isEngagedWithTurret);
        
        
        
        NextStep();
        pullHappened = false;
        ShowCurrentStep(); //secondturret1.5
        yield return new WaitUntil(() => player.turretDetector.detectedTurret);
        yield return new WaitUntil(()=> squatStarted);
        yield return new WaitUntil(() => player.turretDetector.detectsTurret);
        NextStep();
        ShowCurrentStep(); //secondturret2
        yield return new WaitUntil(() => player.isEngagedWithTurret);

        yield return new WaitUntil(() => _turret.chargeCount > 1);
        NextStep();
        ShowCurrentStep(); //wavestarting

       

        EndTutorial();
    }
     /*
    private bool CheckCondition(string step)
    {
        // Here you check the condition based on the step
        // For example:
        switch (step)
        {
            case "introduction":
                return PlayerHasPlacedTower();
            case "startWave":
                return WaveStarted();
            default:
                return false;
        }
    }

    */
  
    private void EndTutorial()
    {
        tutorialPanel.SetActive(false);
        tutorialEnabled = false;
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    // Update or FixedUpdate could be used to trigger tutorial steps based on game conditions
    // For example, moving the player towards a specific location or performing actions
    void Update()
    {
        // Here, you might add checks for player positions or interactions
        // This would be based on your game's logic to automatically advance tutorial steps or respond to player actions
    }

    private void HandlePress(InputAction.CallbackContext context)
    {
        Debug.Log("Press Happened");
        pressHappened = true;
    }

    private void HandleSquatStart(InputAction.CallbackContext context)
    {
        squatStarted = true;
    }

    private void HandleSquatFinish(InputAction.CallbackContext context)
    {
        squatStarted = false;
    }

    private void HandlePull(InputAction.CallbackContext context)
    {

    }
    private void DisplayPressAnimation()
    {
        pressHappened = false;
    }

    private void HidePressAnimation()
    {

    }
    private void DisplaySquatAnimation()
    {
        Debug.Log("Squat Animation triggered");
    }

    private void HideSquatAnimation()
    {

    }

 


}

