using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;






public class InstructionsTextField : MonoBehaviour
{
    private Color greyedOutColor = new Color();
    private Color activeColor = new Color();
    [SerializeField] private InstructionsTextField secondaryTextField;
    private TextMeshProUGUI textMeshProUGUI;

   void Awake()
    {
                greyedOutColor.a = .5f;
        activeColor.a = 1f;
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }


public void SetColorInactive()
{
    textMeshProUGUI.color = greyedOutColor;
}

public void SetText(string text)
{
    textMeshProUGUI.text = text;
}

public void SetText(string text, string secondaryText)
{
    textMeshProUGUI.text = text;
    SetSecondaryText(secondaryText);
}

public void SetColorActive()
{
    textMeshProUGUI.color = activeColor;
}

public void SetSecondaryText(string text)
{
    secondaryTextField.SetText(text);
}

public void HideSecondaryText()
{
    secondaryTextField.enabled = false;
}

public void ShowSecondaryText()
{
    secondaryTextField.enabled = true;
}


}
