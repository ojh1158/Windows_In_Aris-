using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Script.Data;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Range(0.01f,0.2f)]
    public float dialogueSpeed;

    // public float maxWidth;
    
    public TMP_Text dialogueText;
    public RectTransform dialogueRect;
    
    public List<DialogueData> dialogueDataList;

    public void Start()
    {
        StartCoroutine(MemoReal());
    }

    public IEnumerator MemoReal()
    {
        var memoReal = dialogueDataList.Find(data => data.dialogueType == DialogueType.Other);
        
        foreach (var text in memoReal.text)
        {
            yield return StartCoroutine(Dialogue(text));
        }
        
        StartCoroutine(MemoReal());
    }

    public IEnumerator Dialogue(string get_text)
    {
        var text = "";
        var maxWidth = Screen.width - 60;
        
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
        }
        
        yield return new WaitForSeconds(1.5f);
        
        dialogueText.text = "";
        dialogueRect.sizeDelta = new Vector2(0, 0);
    }
}
