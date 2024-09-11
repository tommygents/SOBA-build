using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TurretEntryUIManager : MonoBehaviour
{

    [SerializeField] private TurretEntryIndicator turretEntryIndicator;
    [SerializeField] private TextMeshProUGUI turretActionIndicatorText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera mainCamera;
    private Vector2 canvasPosition;
    public static TurretEntryUIManager Instance;
    private Vector2 turretTextAnchorPosition;
    public RectTransform diagonalLine;
    public RectTransform horizontalLine;
    [SerializeField] private float lineWidth = 2f;
     public Image intersectionCircle; // New field for the circle at the intersection
    private bool isDisplayable = false;
   
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
   }
   
    void Start()
    {
        HideTurretEntryUI();
        turretTextAnchorPosition = turretActionIndicatorText.rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

private Vector2 WorldToCanvasPosition(Vector3 worldPosition)
{
    Vector2 screenPoint = mainCamera.WorldToScreenPoint(worldPosition);
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvas.GetComponent<RectTransform>(),
        screenPoint,
        null,
        out Vector2 canvasPoint);
    return canvasPoint;
}

public void ToggleDisplayable()
{
    isDisplayable = !isDisplayable;
}
public void DisplayTurretEntryUI(Turret turret)
{
    turretEntryIndicator.gameObject.SetActive(true);
    
    // Convert world position to Canvas position
    Vector2 canvasPosition = WorldToCanvasPosition(turret.transform.position);
    
    // Set the anchored position of the RectTransform
    turretEntryIndicator.GetComponent<RectTransform>().anchoredPosition = canvasPosition;

    Debug.Log("Displaying Turret Entry UI at " + turret.transform.position + 
              " with canvas position " + canvasPosition);
    DrawInstructionLine();
}
public void HideTurretEntryUI()
{
    turretEntryIndicator.gameObject.SetActive(false);
    //Also, update the text to return to "Build" and return whatever else to the build context
    HideInstructionLine();
}

private void HideInstructionLine()
{
    diagonalLine.gameObject.SetActive(false);
    horizontalLine.gameObject.SetActive(false);
    intersectionCircle.gameObject.SetActive(false);
}

private void DrawInstructionLine()
    {
        ShowInstructionLine();


        // Convert both positions to canvas space
        Vector2 startPoint = GetCanvasPosition(turretActionIndicatorText.rectTransform);
        Vector2 indicatorPosition = GetCanvasPosition(turretEntryIndicator.GetComponent<RectTransform>());

        // Calculate the center-right point of the indicator
        RectTransform indicatorRect = turretEntryIndicator.GetComponent<RectTransform>();
        Vector2 indicatorSize = indicatorRect.rect.size;
        Vector2 endPoint = indicatorPosition + new Vector2(indicatorSize.x, indicatorSize.y / 2);

        // Calculate the point where the diagonal line meets the horizontal line
        Vector2 middlePoint = new Vector2(startPoint.x - Mathf.Abs(startPoint.y - endPoint.y), endPoint.y);

        // Draw diagonal line
        DrawLine(diagonalLine, startPoint, middlePoint);

        // Draw horizontal line
        DrawLine(horizontalLine, middlePoint, endPoint);
        DrawIntersectionCircle(middlePoint);

        Debug.Log($"Start point: {startPoint}, Middle point: {middlePoint}, End point: {endPoint}");
    }

    private void ShowInstructionLine()
    {
        diagonalLine.gameObject.SetActive(true);
        horizontalLine.gameObject.SetActive(true);
        intersectionCircle.gameObject.SetActive(true);
    }

    private void DrawLine(RectTransform lineTransform, Vector2 start, Vector2 end)
{
    Vector2 direction = end - start;
    float distance = direction.magnitude;

    Debug.Log($"Drawing line from {start} to {end}. Distance: {distance}");

    lineTransform.anchorMin = lineTransform.anchorMax = Vector2.zero;
    lineTransform.sizeDelta = new Vector2(distance, lineWidth);
    lineTransform.anchoredPosition = start + direction * 0.5f;
    lineTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

    Debug.Log($"Line size: {lineTransform.sizeDelta}, position: {lineTransform.anchoredPosition}, rotation: {lineTransform.localEulerAngles}");
}

private Vector2 GetCanvasPosition(RectTransform rectTransform)
    {
        // Convert the local position to world position
        Vector3 worldPosition = rectTransform.TransformPoint(Vector3.zero);

        // Convert world position to canvas position
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPosition);
        Vector2 canvasPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPoint);

        return canvasPoint;
    }

     private void DrawIntersectionCircle(Vector2 position)
    {
        intersectionCircle.rectTransform.anchorMin = intersectionCircle.rectTransform.anchorMax = Vector2.zero;
        intersectionCircle.rectTransform.sizeDelta = new Vector2(lineWidth, lineWidth);
        intersectionCircle.rectTransform.anchoredPosition = position;
    }

}
