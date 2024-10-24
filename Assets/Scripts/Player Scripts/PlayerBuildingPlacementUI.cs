using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class PlayerBuildingPlacementUI : MonoBehaviour
{

    [SerializeField] private Image chargeBarImage;

        [SerializeField] private GameObject radiusCircle;
    [SerializeField] private TextMeshProUGUI deployableText;
    public ChargeBar buildingChargeBar;





    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }




    public void ShowRadius(float _rad)
    {
        radiusCircle.transform.localScale = Vector3.one * _rad * 2f;
        
    }

    public void HideRadius()
    {
        radiusCircle.transform.localScale = Vector3.zero;
    }

    
}
