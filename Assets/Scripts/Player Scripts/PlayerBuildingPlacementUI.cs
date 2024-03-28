using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class PlayerBuildingPlacementUI : MonoBehaviour
{

    [SerializeField] private Image chargeBar;
    [SerializeField] private TextMeshProUGUI chargeCount;
    [SerializeField] private Canvas chargeUICanvas;
    [SerializeField] private CanvasGroup canvasGroup;


    // Start is called before the first frame update
    void Start()
    {
        chargeBar = chargeUICanvas.GetComponentInChildren<Image>();
        chargeCount = chargeUICanvas.GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = chargeUICanvas.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateChargeBar(float _amount, int _chargeCount)
    {
        ShowChargeBar();
        chargeBar.fillAmount = _amount;
        chargeCount.text = _chargeCount.ToString();

    }

    public void HideChargeBar()
    {
        canvasGroup.alpha = 0f;
    }

    public void ShowChargeBar()
    {
        canvasGroup.alpha = 1f;
    }
}
