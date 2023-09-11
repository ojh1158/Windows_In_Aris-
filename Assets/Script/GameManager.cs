using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static bool IsGameMode;

    public void Awake()
    {
        Application.targetFrameRate = 9999;
    }
}
