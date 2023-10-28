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
        if (Input.GetMouseButton(0))
        {
            isGround = false;
            if (Application.isEditor)
            {
                return;
            }
            TransparentApp.API.Pick();
            SchedulerManager.Instance.Pick();
            _gravity = 2;
            return;
        }
        
        
        if (Application.isEditor /*||_isPick*/) return;
        
        isGround = TransparentApp.IsGround();
        
        if (!isGround)
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
