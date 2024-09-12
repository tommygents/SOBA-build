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
    [SerializeField] private TurretSelectionUIIcon zapperTurretUI;
    [SerializeField] private bool zapperMode = false;
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
        UpdateSelectedTurret(turretUIPrefabs[selectedTurretIndex]);    
    }

    void Start()
    {
        HideTurretSelectionWheel(); 
    }


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
        if (!zapperMode)
        {
            turretSelectionTimer = 0f;
        
            if (!turretSelectionActive)
            {
                turretSelectionActive = true;
                ShowTurretSelectionWheel();
                return GetTurretToBuild();
            }
            
            else //if the selection is already active, advance the selection
            {
                AdvanceTurretSelection();
            }
        }

        return GetTurretToBuild();
    }

    
    public Turret GetTurretToBuild()
    {
        if (zapperMode) return zapperTurretUI.GetTurretPrefab();
        else return turretUIPrefabs[selectedTurretIndex].GetTurretPrefab();
    }

    public void UpdateSelectedTurret(TurretSelectionUIIcon _turretUI)
    {
        
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
        if (!zapperMode)
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

    #region Zapper setup

    public void SwitchtoZapper()
    {
        zapperMode = true;
        UpdateSelectedTurret(zapperTurretUI);
        HideTurretSelectionWheel();
    }

    public void SwitchtoTurret()
    {
        zapperMode = false;
        UpdateSelectedTurret(turretUIPrefabs[selectedTurretIndex]);
    }
    #endregion


}
