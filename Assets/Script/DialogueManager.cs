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
        var memoReal = dialogueDataList.Find(data => data.dialogueType == DialogueType.MemoReal);
        
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
    
    public IEnumerator StartRandomWithType(DialogueType dialogueType)
    {
        if (_dialogueCoroutine != null)
        {
            StopCoroutine(Dialogue(""));
            _dialogueCoroutine = null;
        }
        
        var dialogueData = GetDialogueDataOfType(dialogueType);
        var text = dialogueData.text[Random.Range(0, dialogueData.text.Count)];
        yield return _dialogueCoroutine = StartCoroutine(Dialogue(text));
    }

    private IEnumerator Dialogue(string get_text)
    {
        // var text = "";
        var maxWidth = Screen.width - 10;

        var fix = false;
        foreach (var c in get_text)
        {
            dialogueText.text += "_";
            
            var preferredValues = dialogueText.GetPreferredValues();

            var width = preferredValues.x + 10;
            var height = preferredValues.y;
            
            if (!fix && maxWidth  < preferredValues.x)
            {
                fix = true;
            }

            if (fix)
            {
                height += 10;
                width = maxWidth;
            }
            
            dialogueRect.sizeDelta = new Vector2(width, height);
            
            yield return new WaitForSeconds(dialogueSpeed);

            dialogueText.text = dialogueText.text[..^1];
            dialogueText.text += c;
        }
        
        yield return new WaitForSeconds(1.5f);
        
        dialogueText.text = "";
        dialogueRect.sizeDelta = Vector2.zero;
        _dialogueCoroutine = null;
    }

    public void StopAllCoroutineAndIntoText()
    {
        StopAllCoroutines();
        dialogueText.text = "";
        dialogueRect.sizeDelta = Vector2.zero;
        _dialogueCoroutine = null;
    }
}
