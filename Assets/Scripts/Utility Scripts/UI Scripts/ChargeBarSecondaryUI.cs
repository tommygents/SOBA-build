using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBarSecondaryUI : ChargeBarMainUI
{
    public static ChargeBarSecondaryUI Instance { get; private set; }
   
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
            Debug.LogError($"{GetType().Name} already exists, destroying secondary UI");
            Destroy(gameObject);
        }
    }
}
