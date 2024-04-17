using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public int score = 0;
    public int availableMoney = 0;
    public TextMeshProUGUI scoreText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        // Subscribe to the OnEnemyKilled event
        GameEvents.OnEnemyKilled += ScorePoints;
    }

    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        GameEvents.OnEnemyKilled -= ScorePoints;
    }

    public void ScorePoints(int _points)
    {
        score+= _points;
        scoreText.text = "Score: " + score.ToString();
        
    }

    
    //TODO: somehow, this tracks the score going up
    //TODO: This also tracks both earnings (from enemies killed) and expenditures (from turrets built)
    //TODO: 
}
