using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBarPrimaryUI : ChargeBarMainUI
{
    public static ChargeBarPrimaryUI Instance { get; private set; }
  
    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
        else
        {
            Debug.LogError($"{GetType().Name} already exists, destroying primaty UI");
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
}
