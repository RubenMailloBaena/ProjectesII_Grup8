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
    [SerializeField] private float cooldown = 0.5f;  // Tiempo de cooldown
    private float lastExitTime;

    private string currentText = "";
    private Coroutine myCoroutine;
    private bool showedText;
    
    void Start()
    {
        fullText = messageText.text;
        messageText.enabled = false;
        lastExitTime = -cooldown;  // Inicializar para que permita mostrar el mensaje al principio
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !showedText) 
        {
            if (Time.time >= lastExitTime + cooldown)  // Chequea si el cooldown ha pasado
            {
                ShowMessage();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            showedText = false;
            lastExitTime = Time.time;  // Registra el tiempo en que se salió del trigger
            HideMessage();
        }
    }

    void ShowMessage()
    {
        if (messageText != null)
        {
            showedText = true;
            messageText.GetComponent<Animator>().Play("Normal Text Position");
            instance = Instantiate(BocataUI, transform.position, Quaternion.identity);
            instance.GetComponent<Animator>().Play("AppearBocata");
            myCoroutine = StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(0.417f);
        messageText.enabled = true;
        for (int i = 0; i <= fullText.Length; i++)
        {
            Debug.Log("IN COROUTINE");
            currentText = fullText.Substring(0, i);
            messageText.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }

    void HideMessage()
    {
        instance.GetComponent<Animator>().Play("HideBocata");
        messageText.GetComponent<Animator>().Play("Text Hide Animation");
        StartCoroutine(HideBocata());
    }

    IEnumerator HideBocata()
    {
        yield return new WaitForSeconds(0.417f);
        if (myCoroutine != null)
            StopCoroutine(myCoroutine);
        if(instance != null)
            Destroy(instance);
        messageText.enabled = false;
    }
}

// using System.Collections;
// using UnityEngine;
// using TMPro;
//
// public class TriggerMessage : MonoBehaviour
// {
//     [SerializeField]
//     private GameObject BocataUI;
//     private GameObject instance;
//     [SerializeField]
//     private TextMeshProUGUI messageText; 
//     private string fullText;
//
//     [SerializeField] private float delay = 0.5f;
//
//     private string currentText = "";
//     private Coroutine myCoroutine;
//     
//     void Start()
//     {
//         fullText = messageText.text;
//         messageText.enabled = false;
//     }
//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.gameObject.CompareTag("Player")) 
//         {
//             ShowMessage();
//         }
//     }
//
//     void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.gameObject.CompareTag("Player"))
//         {
//             HideMessage();
//         }
//     }
//
//
//     void ShowMessage()
//     {
//         if (messageText != null)
//         {
//             messageText.GetComponent<Animator>().Play("Normal Text Position");
//             instance = Instantiate(BocataUI, transform.position, Quaternion.identity);
//             instance.GetComponent<Animator>().Play("AppearBocata");
//             myCoroutine = StartCoroutine(ShowText());
//         }
//     }
//
//     IEnumerator ShowText()
//     {
//         yield return new WaitForSeconds(0.417f);
//         messageText.enabled = true;
//         for (int i = 0; i <= fullText.Length; i++)
//         {
//             Debug.Log("IN COROUTINE");
//             currentText = fullText.Substring(0, i);
//             messageText.text = currentText;
//             yield return new WaitForSeconds(delay);
//         }
//     }
//
//     void HideMessage()
//     {
//         instance.GetComponent<Animator>().Play("HideBocata");
//         messageText.GetComponent<Animator>().Play("Text Hide Animation");
//         StartCoroutine(HideBocata());
//     }
//
//     IEnumerator HideBocata()
//     {
//         yield return new WaitForSeconds(0.417f);
//         if (myCoroutine != null)
//             StopCoroutine(myCoroutine);
//         if(instance != null)
//             Destroy(instance);
//         messageText.enabled = false;
//     }
// }
//
