using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class PlayerBuildingPlacementUI : MonoBehaviour
{

    [SerializeField] private Image chargeBar;

        [SerializeField] private GameObject radiusCircle;
    [SerializeField] private TextMeshProUGUI deployableText;


    void Start()
    {
        HideDeployableText();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateChargeBar(float _amount)
    {
        
        chargeBar.fillAmount = _amount;
        if(_amount >= 1f)
        {
            ShowDeployableText();
        }
        else
        {
            HideDeployableText();
        }

    }



    public void ShowRadius(float _rad)
    {
        radiusCircle.transform.localScale = Vector3.one * _rad * 2f;
        
    }

    public void HideRadius()
    {
        radiusCircle.transform.localScale = Vector3.zero;
    }

    public void ShowDeployableText()
    {
        deployableText.gameObject.SetActive(true);
    }

    public void HideDeployableText()
    {
        deployableText.gameObject.SetActive(false);
    }
}
