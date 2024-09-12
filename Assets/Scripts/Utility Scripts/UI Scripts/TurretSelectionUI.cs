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
    [SerializeField] private TurretSelectionUIIcon[] turretUIPrefabs;
    [SerializeField] private GameObject turretSelectionWheel;
    [SerializeField] private int selectedTurretIndex;

    public static TurretSelectionUI Instance;

    [SerializeField] private bool turretSelectionActive = false;
    [SerializeField] private float turretSelectionTimer = 0f;
    [SerializeField] private float turretSelectionTimeOut = 1.5f;
    [SerializeField] private float turretSelectionIconMultiplier = 1.25f;

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
    selectedTurretIndex = 0;
    
    
    
    }

    void Start()
    {
        //turretSelectionPanel.SetActive(true);
        UpdateSelectedTurret(turretUIPrefabs[selectedTurretIndex]);
        HideTurretSelectionWheel();
 
    }

    // Update is called once per frame
    void Update()
    {
        IterateTurretSelectionUITimer();
    }

    private void IterateTurretSelectionUITimer()
    {
        if (turretSelectionActive)
        {
            turretSelectionTimer += Time.deltaTime;
            if (turretSelectionTimer > turretSelectionTimeOut)
            {
                turretSelectionActive = false;
                HideTurretSelectionWheel();
            }
        }
    }

    public Turret UpdateSelection() //THIS is the function that returns the turret to build
    {
        turretSelectionTimer = 0f;
       
       if (!turretSelectionActive)
       {
        turretSelectionActive = true;
        
        ShowTurretSelectionWheel();
        return GetTurretToBuild();
       }
       //if the selection is already active, advance the selection
       else
       {
        AdvanceTurretSelection();
        

       }
        return GetTurretToBuild();
    }

    
    public Turret GetTurretToBuild()
    {
        return turretUIPrefabs[selectedTurretIndex].GetTurretPrefab();
    }

    public void UpdateSelectedTurret(TurretSelectionUIIcon _turretUI)
    {
        Debug.Log("TurretUI: " + _turretUI.turretName);
        selectedTurretImage.sprite = _turretUI.icon.sprite;
            // Get the original size of the icon
    Vector2 originalSize = _turretUI.icon.rectTransform.sizeDelta;

    // Calculate the new size (1.25 times larger)
    Vector2 newSize = originalSize * turretSelectionIconMultiplier;

    // Set the new size while maintaining the aspect ratio
    selectedTurretImage.rectTransform.sizeDelta = newSize;

    // Ensure the image uses the new size without stretching
    selectedTurretImage.preserveAspect = true;
        selectedTurretName.text = _turretUI.turretName;
        
    }

    public void AdvanceTurretSelection()
    {
        selectedTurretIndex++;
        if (selectedTurretIndex >= turretUIPrefabs.Length)
        {
            selectedTurretIndex = 0;
        }
        //Move the selection indicator
        turretSelectionIndicator.GetComponent<RectTransform>().position = turretUIPrefabs[selectedTurretIndex].rectTransform.position;
        UpdateSelectedTurret(turretUIPrefabs[selectedTurretIndex]);

    }

    public void ShowTurretSelectionWheel()
    {
        turretSelectionWheel.SetActive(true);
    }

    public void HideTurretSelectionWheel()
    {
        turretSelectionWheel.SetActive(false);
        turretSelectionActive = false;
    }

    


}
