using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifManager : MonoBehaviour
{
    public Image image;
    
    public List<Sprite> spriteList;

    public float speed;
    void Awake()
    {
        StartCoroutine(GIF());
    }
    
    IEnumerator GIF()
    {
        while (true)
        {
            foreach (var sprite in spriteList)
            {
                image.sprite = sprite;
                yield return new WaitForSecondsRealtime(speed);
            }   
        }
    }
}
