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

    private Coroutine _removeText;

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (_removeText != null)
        {
            StopCoroutine(_removeText);
        }
        
        text.text += logString + "\n";
        
        if (type is LogType.Error or LogType.Exception)
        {
            text.text += stackTrace + "\n";
        }

        if (text.text.Length >= 2000)
        {
            text.text = text.text[100..];
        }

        _removeText = StartCoroutine(WaitRemoveText(10f));
    }

    private IEnumerator WaitRemoveText(float time)
    {
        yield return new WaitForSeconds(time);

        text.text = "";
    }
    
    // void Update()
    // {
    //     // 
    //     text.text = Debug;
    // }
}
