using RengeGames.HealthBars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject firstTimePanel;
    public Button yesPlease;
    public Button noThankYou;
    public Button startGame;
    public Button replayTutorial;
    public GameObject mainSelector;
    public GameObject tutSelector;
    private Coroutine squatCoroutine;

    public Button selectedButton;
    public Button unSelectedButton;
    public GameObject selectorObject;
    public bool tutorialButtons = false;

    float pauseSelectionCounter = 0f;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }

    void Start()
    {
        InGameLogger.Instance.Log("Starting MenuManager");

        SubscribeToInputEvents();
        firstTimePanel.SetActive(false);

        Debug.Log(PlayerPrefs.GetInt("Tutorial", -1));
        if (!PlayerPrefs.HasKey("Tutorial") || PlayerPrefs.GetInt("Tutorial") != 1)
        {

            StartCoroutine(FirstTime());
        }

        else
        {
            UseTutorialButtons(false);
        }
    }

private IEnumerator WaitForInputManager()
{
    while (InputManager.Instance == null)
    {
        yield return null; // Wait for the next frame
    }
    SubscribeToInputEvents(); // Subscribe once InputManager is available
}
    private void SubscribeToInputEvents()
    {
        if (InputManager.Instance == null)
    {
        InGameLogger.Instance.LogError("InputManager is not initialized. Retrying...");
        StartCoroutine(WaitForInputManager());
        return;
    }
        InputManager.Instance.OnPull += HandlePull;
        InputManager.Instance.OnSquatStart += HandleSquatStart;
        InputManager.Instance.OnSquatEnd += HandleSquatEnd;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FirstTime()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        firstTimePanel.SetActive(true);
        UseTutorialButtons(true);
        
        
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPull -= HandlePull;
        InputManager.Instance.OnSquatStart -= HandleSquatStart;
        InputManager.Instance.OnSquatEnd -= HandleSquatEnd;
    }

    private void HandlePull(InputAction.CallbackContext ctx)
    {
        Button _temp = selectedButton;
        selectedButton = unSelectedButton;
        unSelectedButton = _temp;
            //selectedButton.Select();
            selectorObject.transform.position = selectedButton.gameObject.transform.position;


          
    }

    private void HandleSquatStart(InputAction.CallbackContext ctx)
    {
        squatCoroutine = StartCoroutine(SquatStart());
    }

    private void HandleSquatEnd(InputAction.CallbackContext ctx)
    {
        if (squatCoroutine != null)
        {
            StopCoroutine(squatCoroutine);
            squatCoroutine = null;  // Clear the reference
        }
        selectedButton.GetComponentInChildren<RadialSegmentedHealthBar>().SetPercent(0f);
    }

    private IEnumerator SquatStart()
    {
        pauseSelectionCounter = 0f;

        // Continue incrementing the counter until it reaches 1 second
        while (pauseSelectionCounter < 1f)
        {
            pauseSelectionCounter += Time.unscaledDeltaTime;
            selectedButton.GetComponentInChildren<RadialSegmentedHealthBar>().SetPercent(pauseSelectionCounter);


            // Yield until the next frame
            yield return null;
        }

        // Once the loop exits, invoke the button click
        selectedButton.GetComponentInChildren<RadialSegmentedHealthBar>().SetPercent(0f);
        selectedButton.onClick.Invoke();
    }

    public void UseTutorialButtons(bool _tut)
    {
        if (_tut)
        {
            selectedButton = yesPlease;
            unSelectedButton = noThankYou;
            selectorObject = tutSelector;

        }
        else
        {
            selectedButton = startGame;
            unSelectedButton = replayTutorial;
            selectorObject = mainSelector;
        }

        Selector();
    }

    public void YesPlease()
    {
        SceneLoader.Instance.LoadGame(true);
    }

    public void NoThankYou()
    {
        firstTimePanel.SetActive(false);
        UseTutorialButtons(false);
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    public void StartTutorial()
    {
        SceneLoader.Instance.LoadGame(true);
    }

    public void StartGame()
    {
        SceneLoader.Instance.LoadGame(false);    
    }

    public void Selector()
    {
        selectorObject.transform.position = selectedButton.gameObject.transform.position;

    }

}
