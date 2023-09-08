using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    
    void Update()
    {
        transform.localPosition += new Vector3(Screen.mainWindowPosition.x, Screen.mainWindowPosition.y);
    }
}
