using System.Collections;
using UnityEngine;
using TMPro;

public class TriggerMessage : MonoBehaviour
{
    [SerializeField]
    private GameObject BocataUI;
    private GameObject instance;
    [SerializeField]
    private TextMeshProUGUI messageText; 
    private string fullText;

    [SerializeField] private float delay = 0.5f;

    private string currentText = "";
    
    void Start()
    {
        fullText = messageText.text;
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
            instance = Instantiate(BocataUI, transform.position, Quaternion.identity);
            instance.GetComponent<Animator>().Play("AppearBocata");
            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(0.417f);
        messageText.enabled = true;
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            messageText.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }

    void HideMessage()
    {
        instance.GetComponent<Animator>().Play("HideBocata");
        messageText.enabled = false;
        StartCoroutine(HideBocata());
    }

    IEnumerator HideBocata()
    {
        yield return new WaitForSeconds(0.417f);
        Destroy(instance);
    }
}

