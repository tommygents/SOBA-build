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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
}
