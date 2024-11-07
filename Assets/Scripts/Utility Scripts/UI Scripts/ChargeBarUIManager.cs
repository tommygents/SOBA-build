using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChargeBarUIManager : MonoBehaviour
{
    [SerializeField] private Image chargeBarImage;
    [SerializeField] private TextMeshProUGUI sectionLabel;
    [SerializeField] private TextMeshProUGUI statusLabel;
    public static ChargeBarUIManager Instance;
    [SerializeField] private ChargeBar currentChargeBar;

    // Start is called before the first frame update
    void Awake()
    {
    if (Instance == null)
    {
    Instance = this;
    DontDestroyOnLoad(gameObject);
    Debug.Log("ChargeBarUIManager Awake");
    }
    else
    {
    Destroy(gameObject);
    }
    }
    void Start()
    {
        //HideStatusText();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateChargeBar();
    }

    public void ShowStatusText()
    {
        statusLabel.gameObject.SetActive(true);
    }

    public void HideStatusText()
    {
        statusLabel.gameObject.SetActive(false);
    }

    public void FillChargeBarImage(float _amount)
    {
        chargeBarImage.fillAmount = _amount;
    }

    public void SwitchTo(ChargeBar _chargeBar)
    {
        currentChargeBar = _chargeBar;
        chargeBarImage.color = _chargeBar.chargeColor;
        sectionLabel.text = _chargeBar.sectionName;
        
        if (_chargeBar.AtMaxLevel())
        {
            statusLabel.text = "Max Level!";
        }
        else
        {
            statusLabel.text = _chargeBar.statusName;
        }
        FillChargeBarImage(currentChargeBar.GetChargeAmount());    
    }

    public void UpdateChargeBar()
    {
        if (currentChargeBar.AtMaxLevel())
        {
            statusLabel.text = "Max Level!";
        }
        else
        {
            statusLabel.text = currentChargeBar.statusName;
        }
        FillChargeBarImage(currentChargeBar.GetChargeAmount());
        if(currentChargeBar.IsFull())
        {
            ShowStatusText();
        }
        else
        {
            HideStatusText();
        }
    }
}
