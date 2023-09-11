using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUi : MonoBehaviour
{
    public TMP_Text text;

    public static string Debug;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Debug;
    }
}
