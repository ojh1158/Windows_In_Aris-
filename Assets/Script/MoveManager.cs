using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Application = UnityEngine.Application;

public class MoveManager : MonoBehaviour
{
    public static MoveManager In;
    
    public RectTransform pickRectTransform;
    public Transform arisTransform;

    public static bool isGround;
    private void Awake()
    {
        In = this;
    }

    private bool _isPick;
    
    private float _gravity = 2;
    // private float _gravityWeighted;

    public void Update()
    {
        if (!MenuManager.IsMenuOpen && Input.GetMouseButton(0))
        {
            isGround = false;
            if (Application.isEditor)
            {
                return;
            }
            TransparentApp.API.Pick();
            _gravity = 2;
            // _isPick = true;
            return;
        }
        // else
        // {
        //     _isPick = false;
        // }
        
        if (Application.isEditor /*||_isPick*/) return;
        
        isGround = TransparentApp.IsGround();
        
        if (!isGround)
        {
            // _gravityWeighted += Time.deltaTime;
            _gravity += Time.deltaTime * 20;
            // DebugUi.Debug = _gravity.ToString("F2");
            var rect = TransparentApp.GetLeftUpVector2();
            TransparentApp.API.Move((int)rect.x ,(int)rect.y + (int)_gravity);
        }
        else
        {
            _gravity = 2;
        }
    }
    
    public void SetRotate(int rotateY)
    {
        arisTransform.eulerAngles = new Vector3(0, rotateY, 0);
    }
}
