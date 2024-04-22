using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour, IWaterEffect
{
    private Color effectColor;
    private ColorType colorType;

    private Color previousColor;

    public static event Action onWater;

    public WaterEffect(Color color, ColorType colorType) { 
        effectColor = color;
        this.colorType = colorType; 
    }

    public void InitializeEffect(GameObject target)
    {
        previousColor = target.GetComponent<SpriteRenderer>().color;
        target.GetComponent<SpriteRenderer>().color = effectColor;
        target.GetComponent<BoxCollider2D>().isTrigger = true;

        //COMPROBEN QUE AL INICIALITZAR EL JUGADOR NO ESTIGUI A SOBRE
        Collider2D[] collider = Physics2D.OverlapBoxAll(target.transform.position, target.transform.localScale, 0f);

        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].gameObject.CompareTag("Player"))
            {
                onWater?.Invoke();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
<<<<<<< HEAD
        // Verifica si el objeto entrante es el jugador u otro objeto de interés
        if (other.gameObject.tag == "Player") // Asegúrate de que el jugador tenga asignado el tag "Player"
        {
            ApplyEffect(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Verifica si el objeto saliente es el jugador u otro objeto de interés
        if (other.gameObject.tag == "Player")
        {
            RemoveEffect(other.gameObject);
        }
    }

    public void ApplyEffect(GameObject target)
    {
        // Guarda el color original solo si no se ha aplicado previamente el efecto
        if (previousColor == Color.clear) // Color.clear es solo un ejemplo, necesitarás una mejor lógica para manejar múltiples objetos
        {
            previousColor = target.GetComponent<SpriteRenderer>().color;
        }

        target.GetComponent<SpriteRenderer>().color = effectColor;
        target.GetComponent<BoxCollider2D>().isTrigger = true;

        // Aquí podrías añadir otros efectos como alterar la gravedad, etc.
        Debug.Log("Aplicando efecto de agua");
    }

    public void RemoveEffect(GameObject target)
    {
        target.GetComponent<SpriteRenderer>().color = previousColor;
        target.GetComponent<BoxCollider2D>().isTrigger = false;

        // Restablecer otros efectos aquí
        Debug.Log("Removiendo efecto de agua");
=======
        onWater?.Invoke();
>>>>>>> Second-Version
    }

    public ColorType getColorType()
    {
        return colorType;
    }

<<<<<<< HEAD
=======
    public void RemoveEffect(GameObject target)
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(target.transform.position, target.transform.localScale, 0f);

        for (int i = 0; i < collider.Length; i++) {
            if (collider[i].gameObject.CompareTag("Player")) {
                onWater?.Invoke();
            }
        }
        target.GetComponent<SpriteRenderer>().color = previousColor;
        target.GetComponent<BoxCollider2D>().isTrigger = false;
    }
>>>>>>> Second-Version
}
