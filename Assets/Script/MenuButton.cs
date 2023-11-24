using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public Button button; 
    public TMP_Text buttonText;
    public Animation menuAnimation;

    private Func<IEnumerator> _action;
    private Func<int ,IEnumerator> _actionMonitor;
    private int _monitorNum;
    
    public MenuButton SetButton(string text, Func<IEnumerator> action)
    {
        buttonText.text = text;

        _action = action;

        return this;
    }

    public MenuButton SetMonitorButton(string text, int num, Func<int ,IEnumerator> action)
    {
        buttonText.text = text;

        _actionMonitor = action;

        _monitorNum = num;
        return this;
    }

    public void Action()
    { 
        MoveManager.Instance.StartCoroutine(_action != null ? _action() : _actionMonitor(_monitorNum));
    }
}
