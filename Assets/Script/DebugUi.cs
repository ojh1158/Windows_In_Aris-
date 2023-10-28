using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUi : MonoBehaviour
{
    public TMP_Text text;

    public static string Debug;
    
    void Update()
    {
        // 
        text.text = Debug;
    }
}
