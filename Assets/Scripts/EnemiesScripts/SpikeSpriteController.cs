using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpriteController : MonoBehaviour
{
    [SerializeField] int spriteSelected = 1;
    [SerializeField] Sprite[] possibleSprites;
    private void Start()
    {

        if (spriteSelected > possibleSprites.Length - 1 || spriteSelected < 0)
            GetComponent<SpriteRenderer>().sprite = possibleSprites[Random.Range(0,6)];
        else
            GetComponent<SpriteRenderer>().sprite = possibleSprites[spriteSelected];


        GetComponent<SpriteMask>().sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
