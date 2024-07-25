using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretSelectionUI : MonoBehaviour
{

    [SerializeField] private Image selectedTurretImage;
    [SerializeField]  private TextMeshProUGUI selectedTurretName;
    [SerializeField] private GameObject turretSelectionIndicator;
    [SerializeField] private GameObject turretSelectionPanel;
    [SerializeField] private TurretImage[] turretImagePrefabs;
    [SerializeField] private GameObject turretSelectionWheel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO:
    // Advance the selection indicator around the wheel
    // Update the selected turret image and name
    // Show and hide the wheel

    public void AdvanceSelectionIndicator()
    {
        //TODO: Implement this
    }

    public void UpdateSelectedTurret(Turret turret)
    {
        //TODO: Implement this
    }

    public void ShowTurretSelectionWheel()
    {
        //TODO: Implement this
    }

    public void HideTurretSelectionWheel()
    {
        //TODO: Implement this
    }
}
