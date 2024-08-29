using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class TurretImage : Image
{
    [SerializeField] public Turret turretPrefab;
    [SerializeField] public string turretName;

    public Turret TurretPrefab { get => turretPrefab; set => turretPrefab = value; }
    public string TurretName { get => turretName; set => turretName = value; }

    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Turret GetTurretPrefab()
    {
        return turretPrefab;
    }

    public Vector2 GetRectTransformPosition()
    {
        return rectTransform.anchoredPosition;
    }
}