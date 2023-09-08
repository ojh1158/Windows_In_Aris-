using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    private Image this_image;
    private void Awake()
    {
        this_image = GetComponent<Image>();
    }
    
    private void OnCollisionEnter() 
    {
         
    }

    IEnumerator ColorFade()
    {
        while (true)
        {
            
        }
    }
}
