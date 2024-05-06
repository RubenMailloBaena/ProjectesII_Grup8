using UnityEngine;
using UnityEngine.UI;


public class TextTrigger : MonoBehaviour
{
    [Tooltip("Referencia al objeto TextLogic que controla el display")]
    [SerializeField]
    public TextLogic textLogic;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textLogic.ShowText();
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textLogic.HideText();
        }
    }
}
