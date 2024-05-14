using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Renderer targetRenderer;

    private Color initialColor = new Color(0.396f, 0.4f, 0.396f, 1f);
    // Start is called before the first frame update
    void Start()
    {
       
        
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("u"))
        {
            Material material = targetRenderer.material;
            material.SetColor("ColorReference", initialColor);
            material.SetColor("ColorChanger", Color.yellow);
        }
    }
}
