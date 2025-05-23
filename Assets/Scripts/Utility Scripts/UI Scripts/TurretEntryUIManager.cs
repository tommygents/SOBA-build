using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public RectTransform instructionHighlightBox;
    [SerializeField] private float lineWidth = 2f;
     public Image intersectionCircle; // New field for the circle at the intersection
    private bool isDisplayable = false;
    private Color dimColor = new Color(1, 1, 1, 0.5f);
    private Color normalColor = new Color(1, 1, 1, 1);
    [SerializeField] private RectTransform turretEntryUIPanel;
   
   void Awake()
   {
 if (Instance == null)
    {
    Instance = this;
    DontDestroyOnLoad(gameObject);
    SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to scene loaded event
    }
    else
    {
    Destroy(gameObject);
    }
   }

   private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
   {
    HideTurretEntryUI();
    turretTextAnchorPosition = turretActionIndicatorText.rectTransform.anchoredPosition;
    normalColor = instructionHighlightBox.GetComponent<Image>().color;
    dimColor = new Color(normalColor.r, normalColor.g, normalColor.b, 0.5f);
   }

   private void OnDestroy()
   {
    SceneManager.sceneLoaded -= OnSceneLoaded;  // Clean up subscription
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

// New method
private Vector2 WorldToSpecificCanvasPosition(Vector3 worldPosition, RectTransform targetRectTransform)
{
    // Convert world position to screen position
    Vector2 screenPoint = mainCamera.WorldToScreenPoint(worldPosition);

    // Convert screen position to local position within the target RectTransform
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        targetRectTransform,
        screenPoint,
        null,
        out Vector2 localPoint);

    return localPoint;
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

  
    DrawIndicatorLine();
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
    instructionHighlightBox.gameObject.SetActive(false);
}


private void DrawIndicatorLine()
{
    ShowInstructionLine();
    //Figure out where to start the diagonal line from
    //RectTransform textRect = turretActionIndicatorText.rectTransform;
    Vector2 anchorPos = instructionHighlightBox.anchoredPosition;
    Vector2 textSize = instructionHighlightBox.rect.size;
    Vector2 textRectLocalPosition = turretEntryUIPanel.InverseTransformPoint(instructionHighlightBox.TransformPoint(Vector3.zero));
    Vector2 startPoint = anchorPos + new Vector2(-textSize.x/2, textSize.y/2);
    //Get the indicator position, which is also the endpoint
    RectTransform indicatorRect = turretEntryIndicator.GetComponent<RectTransform>();
    Vector2 indicatorPosition = turretEntryUIPanel.InverseTransformPoint(indicatorRect.TransformPoint(Vector3.zero));
    Vector2 indicatorSize = indicatorRect.rect.size;
    //Get the endpoint by moving from the indicator position
    Vector2 endPoint = indicatorPosition + new Vector2(indicatorSize.x, indicatorSize.y / 2);
    //Get the midpoint, where the diagonal line ends
    Vector2 middlePoint = new Vector2(startPoint.x - Mathf.Abs(startPoint.y - endPoint.y), endPoint.y);
    // Draw diagonal line
        DrawLine(diagonalLine, startPoint, middlePoint);

        // Draw horizontal line
        DrawLine(horizontalLine, middlePoint, endPoint);
        DrawIntersectionCircle(middlePoint);
}
private void DrawInstructionLine()
    {
        ShowInstructionLine();


        // Convert both positions to canvas space
       // Get the left-edge of the indicator text 
        RectTransform textRect = turretActionIndicatorText.rectTransform;
        Vector2 textSize = textRect.rect.size;
        Vector2 startPoint = textRect.TransformPoint(Vector2.zero) + new Vector3(-textSize.x/2, textSize.y/2, 0);
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
        instructionHighlightBox.gameObject.SetActive(true);
    }

    private void DrawLine(RectTransform lineTransform, Vector2 start, Vector2 end)
{
    Vector2 direction = end - start;
    float distance = direction.magnitude;



    lineTransform.anchorMin = lineTransform.anchorMax = Vector2.zero;
    lineTransform.sizeDelta = new Vector2(distance, lineWidth);
    lineTransform.anchoredPosition = start + direction * 0.5f;
    lineTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);


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

    private void GetLocalizedPoint(Vector2 _point)
    {
        
    
    }

    public void DimInstructionLine()
    {
        diagonalLine.GetComponent<Image>().color = dimColor;
        horizontalLine.GetComponent<Image>().color = dimColor;
        intersectionCircle.GetComponent<Image>().color = dimColor;
        instructionHighlightBox.GetComponent<Image>().color = dimColor;
        turretEntryIndicator.GetComponent<Image>().color = dimColor;
    }

    public void NormalizeInstructionLine()
    {
        diagonalLine.GetComponent<Image>().color = normalColor;
        horizontalLine.GetComponent<Image>().color = normalColor;
        intersectionCircle.GetComponent<Image>().color = normalColor;
        instructionHighlightBox.GetComponent<Image>().color = normalColor;
        turretEntryIndicator.GetComponent<Image>().color = normalColor;
    }
}
