using RengeGames.HealthBars;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;  // Assign in inspector or find dynamically
    public CameraMovement cameraMovement;  // Assign in inspector or find dynamically
    public GameObject pauseScreen;
    public Button resumeButton;
    public Button restartButton;
    public GameObject selectionOutline;
    public bool isPaused = false;
    public bool resumeButtonSelected = true;
    public InputManager inputManager;
    public Button selectedButton;
    public float pauseSelectionCounter;
    public Coroutine squatCoroutine;

    public GameObject controlMenu;
    public GameObject[] pushElements;
    public TextMeshProUGUI squatText;
    public TextMeshProUGUI pullText;
    public string squatCache;
    public string pullCache;


    void Awake()
    {
        Time.timeScale = 1.0f;
        WaveManager.OnWaveStart += HandleWaveStart;
        WaveManager.OnWaveEnd += HandleWaveEnd;
        selectedButton.Select();
        pauseScreen.SetActive(false);
        //InputManager.Instance.OnPause += Pause;
        isPaused = false;
        
    }

    void OnDisable()
    {
        //InputManager.Instance.OnPause -= Pause;
        InputManager.Instance.OnPull -= HandlePull;
        InputManager.Instance.OnSquatStart -= HandleSquatStart;
        InputManager.Instance.OnSquatEnd -= HandleSquatEnd;
    }
    private void HandleWaveStart(int waveNumber)
    {
        player.HidePlayerDuringWave(waveNumber);
        player.gameObject.SetActive(false);
        cameraMovement.CenterCameraOnField(waveNumber);
    }

    private void HandleWaveEnd()
    {
        player.gameObject.SetActive(true);
        player.ResetPositionToLastSaved();  // Make sure the Player script can handle this
        cameraMovement.SetCameraToFollowPlayer(player);
    }

    public void Pause()
    {
        isPaused = !isPaused;
        

        if (isPaused)
        {
            InputManager.Instance.OnPull += HandlePull;
            InputManager.Instance.OnSquatStart += HandleSquatStart;
            InputManager.Instance.OnSquatEnd += HandleSquatEnd;
            foreach(GameObject _go in pushElements)
            {
                _go.SetActive(false);
            }
            pullCache = pullText.text;
            pullText.text = "Select";
            squatCache = squatText.text;
            squatText.text = "Confirm";

        }
        else
        {
            InputManager.Instance.OnPull -= HandlePull;
            InputManager.Instance.OnSquatStart -= HandleSquatStart;
            InputManager.Instance.OnSquatEnd -= HandleSquatEnd;
            foreach (GameObject _go in pushElements)
            {
                _go.SetActive(true);
                pullText.text = pullCache;
                squatText.text = squatCache;
                pullCache = null;
                squatCache = null;
            }
        }
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(isPaused); // Show or hide the pause menu
            //selectedButton.Select();
        }

        Time.timeScale = isPaused ? 0 : 1; // Pause or unpause the game

    }

    public void Restart()
    {
        SceneLoader.Instance.LoadMenu();
    }

    private void HandleSquat()
    {

    }
    
    private void HandlePull(InputAction.CallbackContext ctx)
    {
        if (isPaused)
        {
            
            resumeButtonSelected = !resumeButtonSelected;
            selectedButton = resumeButtonSelected ? resumeButton : restartButton;
            selectedButton.Select();
            selectionOutline.transform.position = selectedButton.gameObject.transform.position;    

            
            

        }
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


}