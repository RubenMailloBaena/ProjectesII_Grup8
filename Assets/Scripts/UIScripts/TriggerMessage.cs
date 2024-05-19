using UnityEngine;
using TMPro;

public class TriggerMessage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI messageText; 
    [SerializeField]
    private string message = "¡Has entrado en la zona!";

    void Start()
    {
        messageText.enabled = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            ShowMessage();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HideMessage();
        }
    }


    void ShowMessage()
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.enabled = true;
            Invoke("HideMessage", 4f); 
        }
    }

    void HideMessage()
    {
            messageText.enabled = false;
        
    }
}

