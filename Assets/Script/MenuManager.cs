using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("MenuGameObject")] 
    public GameObject menuGameObject;
    
    
    public static bool IsMenuOpen;

    public void Update()
    {
        if (!Input.GetMouseButton(1)) return;

        IsMenuOpen = !IsMenuOpen;
        menuGameObject.SetActive(IsMenuOpen);
    }

    public void CloseMenu()
    {
        IsMenuOpen = !IsMenuOpen;
        menuGameObject.SetActive(IsMenuOpen);
    }
    
    public void StartMove()
    {
        
    }

    public void StopMove()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }
    
}
