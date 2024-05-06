using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class TextLogic : MonoBehaviour
{
    [Tooltip("Aqui va el texto de UI que muestra el mensaje")]
    public TMP_Text textField;

    private void Start()
    {
        HideText();
    }

    public void ShowText()
    {
        textField.enabled = true;
    }

    public void HideText()
    {
        textField.enabled = false;
    }
}
