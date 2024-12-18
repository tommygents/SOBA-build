using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChargeBarUIManager : ChargeBarMainUI
{
    public static ChargeBarMainUI Instance { get; private set; }
   
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
            Debug.LogError($"{GetType().Name} already exists, destroying building UI");
            Destroy(gameObject);
        }
    }
} 