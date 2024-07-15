using RengeGames.HealthBars;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public int score = 0;
    public int availableMoney = 0;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI congratulationsText;
    public Button selectedButton;
    public GameObject selectionObject;
    public float pauseSelectionCounter;
    public Coroutine squatCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        // Subscribe to the OnEnemyKilled event
        GameEvents.OnEnemyKilled += ScorePoints;
        WaveManager.OnEndGame += GameEnd;
    }

    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        GameEvents.OnEnemyKilled -= ScorePoints;


        InputManager.Instance.OnPullStart -= HandleSquatStart;
        InputManager.Instance.OnPullEnd -= HandleSquatEnd;
        InputManager.Instance.OnSquatStart -= HandleSquatStart;
        InputManager.Instance.OnSquatEnd -= HandleSquatEnd;

    }

    public void ScorePoints(int _points)
    {
        score += _points;
        scoreText.text = score.ToString();

    }

    public void GameEnd()
    {
        gameOverScreen.SetActive(true);
        currentScoreText.text = score.ToString();
        int _highScore = PlayerPrefs.GetInt("Score", 0);
        if (score > _highScore)
        {
            _highScore = score;
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.Save();
            congratulationsText.text = "New high score!";


        }
        else { congratulationsText.text = "Good job!"; }

        highScoreText.text = _highScore.ToString();
        InputManager.Instance.OnPullStart += HandleSquatStart;
        InputManager.Instance.OnPullEnd += HandleSquatEnd;
        InputManager.Instance.OnSquatStart += HandleSquatStart;
        InputManager.Instance.OnSquatEnd += HandleSquatEnd;



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

    public void ReturnToMenu()
    {
        SceneLoader.Instance.LoadMenu();
    }

}
