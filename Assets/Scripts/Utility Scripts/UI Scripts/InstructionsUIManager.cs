using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionsUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static InstructionsUIManager Instance;
    [SerializeField] public InstructionsTextField pullText;
    [SerializeField] public InstructionsTextField pushText;
    [SerializeField] public InstructionsTextField squatText;
    [SerializeField] public InstructionsTextField runText;
    private Animator animator;
    [SerializeField] private GameObject runningSprite;



    void Awake()
    {
        if (Instance == null)
        {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        }
        else
        {
        Destroy(gameObject);
        }
    }
    
    void Start()
    {
        animator = runningSprite.GetComponent<Animator>();
        runningSprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        public void StartRunning()
    {
        runningSprite.SetActive(true);
        animator.Play("RunningAnimation"); // Use your animation clip's name here
        runText.SetText("Charge Turret", "Sprint for bonus");
        // OR
        //animator.SetBool("IsRunning", true); // If using a parameter
    }

    // Method to stop the animation
    public void StopRunning()
    {
        runningSprite.SetActive(false);
        // OR
        //animator.SetBool("IsRunning", false); // If using a parameter
        runText.SetText("", "");
    }
    
}
