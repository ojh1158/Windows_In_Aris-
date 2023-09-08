using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public string text_string;
    public float speed;
    public TMP_Text text;

    public void Awake()
    {
        Application.targetFrameRate = 9999;
    } 

    bool _toggle;
    public void Update()
    {
        // text.text = $"{text_string}";
        // text.color = Color.HSVToRGB(Mathf.PingPong(Time.time * speed, 1), 1, 1);
    }
}
