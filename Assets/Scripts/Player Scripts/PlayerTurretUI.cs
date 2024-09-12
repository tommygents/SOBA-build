using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTurretUI : MonoBehaviour
{
    [SerializeField] public GameObject towerSelectionPanel;
    [SerializeField] public TurretImage[] turretPrefabs;
    private List<TurretImage> turretBuildListItems = new List<TurretImage>();
    public int selectedIndex = 0;
    public GameObject selectionHighlightBorder;
    public TextMeshProUGUI turretName;
    [SerializeField] private float unitSize = .1f; // Base unit size
    [SerializeField] private float paddingInUnits = 1f; // Padding in units
    [SerializeField] private float turretSizeInUnits = 10f; // Size of turret image in unit
    [SerializeField] private Vector2 unitSpacing = new Vector2(1f, 1f);

    private void Awake()
    {
        
        selectedIndex = 0;
        
        InitializeTurretListFromArray();
        //HideTowerSelectionPanel();
        
        UpdateHighlightPosition();
        
    }


    public void PlaceTurretImages()
    {
//TODO: make sure the spacing works out more cleanly

        int _width = Mathf.Min(turretPrefabs.Length, 4);
        int _height = Mathf.CeilToInt(turretPrefabs.Length / 4f);   
      
        unitSpacing.x = selectionHighlightBorder.transform.localScale.x;
        unitSpacing.y = selectionHighlightBorder.transform.localScale.y;    
        Vector2 _startingOffset = new Vector2(-towerSelectionPanel.GetComponent<RectTransform>().rect.width/2 + (unitSpacing.x / 2f), -towerSelectionPanel.GetComponent<RectTransform>().rect.height/2 + (unitSpacing.y / 2f));
        Vector2 _padding = new Vector2(paddingInUnits * unitSpacing.x, paddingInUnits * unitSpacing.y);
      
        int _turretCount = 0;
        for (int i = 0; i < _height; i++)
            for (int j = 0; j < _width; j++)
            {
               
                if (_turretCount < turretPrefabs.Length)
                    {
                turretBuildListItems.Add(PlaceTurretImage(turretPrefabs[_turretCount], _startingOffset + new Vector2(j * (unitSpacing.x + _padding.x) * 2f, i * (unitSpacing.y + _padding.y))));
                _turretCount++;
                    }
            }
    }

    private TurretImage PlaceTurretImage(TurretImage _turret, Vector2 _position)
    {

        TurretImage _turretInstance = Instantiate(_turret, _position, Quaternion.identity, towerSelectionPanel.transform);
        return _turretInstance;
        
    }
    private void InitializeTurretListFromArray()
    {
        foreach (TurretImage turret in turretPrefabs)
        {
            turretBuildListItems.Add(turret);
        }
    }

    public void InitializeTurretList() //No longer using this one
    {
        int _width = Mathf.Min(turretPrefabs.Length, 4);
        int _height = Mathf.CeilToInt(turretPrefabs.Length / 4f);

        RectTransform panelRect = towerSelectionPanel.GetComponent<RectTransform>();
        float panelWidth = (_width * turretSizeInUnits + (_width + 1) * paddingInUnits) * unitSize;
        float panelHeight = (_height * turretSizeInUnits + (_height + 1) * paddingInUnits) * unitSize;
        panelRect.sizeDelta = new Vector2(panelWidth, panelHeight);

        float startX = -panelWidth / 2 + (paddingInUnits + turretSizeInUnits / 2) * unitSize;
        float startY = panelHeight / 2 - (paddingInUnits + turretSizeInUnits / 2) * unitSize;

        for (int i = 0; i < turretPrefabs.Length; i++)
        {
            int row = i / 4;
            int col = i % 4;

            float xPos = startX + col * (turretSizeInUnits + paddingInUnits) * unitSize;
            float yPos = startY - row * (turretSizeInUnits + paddingInUnits) * unitSize;

            TurretImage turretInstance = Instantiate(turretPrefabs[i], new Vector2(xPos, yPos), Quaternion.identity, towerSelectionPanel.transform);
            turretBuildListItems.Add(turretInstance);



        }
    }

    public void ShowTowerSelectionPanel()
    {
        towerSelectionPanel.SetActive(true);
        turretName.gameObject.SetActive(true);
        UpdateHighlightPosition();
    }

    public void HideTowerSelectionPanel()
    {
        towerSelectionPanel.SetActive(false);
        turretName.gameObject.SetActive(false);
    }


    public void IterateSelection()
    {
        selectedIndex = (selectedIndex + 1) % turretBuildListItems.Count;
        UpdateHighlightPosition();
        Debug.Log($"Selected turret index: {selectedIndex}");
    }

    public void UpdateHighlightPosition()
    {
        if (selectedIndex >= 0 && selectedIndex < turretBuildListItems.Count)
        {
            TurretImage selectedTurret = turretBuildListItems[selectedIndex];
            selectionHighlightBorder.transform.position = selectedTurret.transform.position;
      

            turretName.text = selectedTurret.turretName;
            Debug.Log($"Updated highlight for turret {selectedIndex}: {selectedTurret.name}");
        }
        else
        {
            Debug.LogWarning($"Invalid selectedIndex: {selectedIndex}. Total turrets: {turretBuildListItems.Count}");
        }
    }


    public Turret MakeTurretSelection()
    {
        if (selectedIndex >= 0 && selectedIndex < turretBuildListItems.Count)
        {
            Turret _turret = (Turret)turretBuildListItems[selectedIndex].GetTurretPrefab();
            //HideTowerSelectionPanel();
            Debug.Log($"Selected turret {selectedIndex}: {_turret.name}");
            return _turret;
        }
        else
        {
            Debug.LogError($"Cannot make selection. Invalid selectedIndex: {selectedIndex}. Total turrets: {turretBuildListItems.Count}");
            return null;
        }
    }
}
        