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

    public static bool IsGround;
    public static bool IsPick;
    private void Awake()
    {
        In = this;
    }
    
    private float _gravity = 2;
    // private float _gravityWeighted;

    public void Update()
    {
        IsPick = Input.GetMouseButton(0);
        if (!MenuManager.IsMenuOpen && IsPick)
        {
            IsGround = false;
            StartCoroutine(SchedulerManager.Instance.Pick());
            if (Application.isEditor)
            {
                return;
            }
            TransparentApp.API.Pick();
            _gravity = 2;
            return;
        }
        
        if (Application.isEditor) return;

        IsGround = TransparentApp.IsGround();
        
        if (!IsGround)
        {
            _gravity += Time.deltaTime * 20;
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
