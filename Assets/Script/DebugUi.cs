using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUi : MonoBehaviour
{
    public TMP_Text text;
    
    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        text.text += logString + "\n";
        // 선택적: 오류 및 경고의 경우 스택 추적도 표시
        if (type == LogType.Error || type == LogType.Exception)
        {
            text.text += stackTrace + "\n";
        }

        if (text.text.Length >= 2000)
        {
            text.text = text.text[100..];
        }
    }
    
    // void Update()
    // {
    //     // 
    //     text.text = Debug;
    // }
}
