using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Script.Data;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    
    [Range(0.01f,0.2f)]
    public float dialogueSpeed;

    // public float maxWidth;
    
    public TMP_Text dialogueText;
    public RectTransform dialogueRect;
    
    public List<DialogueData> dialogueDataList;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        //StartCoroutine(MemoReal());
    }

    private IEnumerator MemoReal()
    {
        var memoReal = dialogueDataList.Find(data => data.dialogueType == DialogueType.Other);
        
        foreach (var text in memoReal.text)
        {
            yield return StartCoroutine(Dialogue(text));
        }
        
        StartCoroutine(MemoReal());
    }

    private DialogueData GetDialogueDataOfType(DialogueType dialogueType)
    {
        return dialogueDataList.Find(data => data.dialogueType == dialogueType);
    }

    private Coroutine _dialogueCoroutine;
    
    public void StartRandomWithType(DialogueType dialogueType)
    {
        if (_dialogueCoroutine != null)
        {
            StopCoroutine(Dialogue(""));
            _dialogueCoroutine = null;
        }
        
        var dialogueData = GetDialogueDataOfType(dialogueType);
        var text = dialogueData.text[Random.Range(0, dialogueData.text.Count)];
        _dialogueCoroutine = StartCoroutine(Dialogue(text));
    }

    private IEnumerator Dialogue(string get_text)
    {
        var text = "";
        var maxWidth = 140;
        
        float fix = 0;
        
        for (var i = 0; i < get_text.Length; i++)
        {
            dialogueText.text += "_";
            
            Vector2 preferredValues = dialogueText.GetPreferredValues();

            float width = preferredValues.x;
            float height = preferredValues.y;

            if (fix != 0)
            {
                width = fix;
            }
            
            if (fix == 0 && maxWidth < preferredValues.x + 20)
            {
                fix = maxWidth;
                width = maxWidth;
            }
            
            dialogueRect.sizeDelta = new Vector2(width + 30, height + 10);
            
            dialogueText.text = dialogueText.text.PadLeft(1);
            
            yield return new WaitForSeconds(dialogueSpeed);
            
            text += get_text[i];
            dialogueText.text = text;

            _dialogueCoroutine = null;
        }
        
        yield return new WaitForSeconds(1.5f);
        
        dialogueText.text = "";
        dialogueRect.sizeDelta = new Vector2(0, 0);
    }
}
