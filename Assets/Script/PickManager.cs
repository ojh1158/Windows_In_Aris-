using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickManager : MonoBehaviour
{
    public static PickManager In;
    public RectTransform pickRectTransform;

    private void Awake()
    {
        In = this;
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Application.isEditor)
            {
                return;
            }
            TransparentApp.API.Pick();
        }
    }
}
