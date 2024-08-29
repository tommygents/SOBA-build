using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class TurretSelectionUIIcon : MonoBehaviour
{
    public Image icon;
    public Turret turretPrefab;
    public string turretName;

    public RectTransform rectTransform;

    // Start is called before the first frame update
    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        icon = GetComponent<Image>();
        
    }   
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Turret GetTurretPrefab()
    {
        return turretPrefab;
    }   
}
