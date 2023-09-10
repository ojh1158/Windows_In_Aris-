using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickManager : MonoBehaviour
{
    // public void Pick()
    // {
    //     Debug.Log("VAR");
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Debug.Log("in");
    //
    //         if (Application.isEditor)
    //         {
    //             return;
    //         }
    //         TransparentApp.Pick();
    //     }
    // }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("in");

            if (Application.isEditor)
            {
                return;
            }
            TransparentApp.API.Pick();
        }
    }
}
