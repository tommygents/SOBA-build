using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public static SceneLoader Instance;
    public bool TutorialEnabled { get; private set; }
    


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make this object persistent across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadGame(bool _tutorial)
    {
        TutorialEnabled = _tutorial;
        SceneManager.LoadScene("TD Build");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu Scene");
    }

    public void SetTutorialEnabled(bool isEnabled)
    {
        TutorialEnabled = isEnabled;
    }



}
