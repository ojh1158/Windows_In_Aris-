using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MenuManager : MonoBehaviour
{
    [Header("MenuGameObject")] 
    public GameObject menuGameObject;

    [Header("Prefab")] 
    public GameObject menuButton;

    [Header("MenuTransForm")] 
    public Transform menuTransform;
    
    [Header("Animation")] 
    public AnimationClip leftClip;
    public AnimationClip rightClip;

    [Header("Button")] 
    public List<Animation> leftAnimationList;
    public List<Animation> rightAnimationList;


    public static bool IsCreateMenuOpen;
    public static bool IsMenuOpen;

    public void Start()
    {
        StartCoroutine(WaitForRightClick());
    } 

    private IEnumerator WaitForRightClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (!IsCreateMenuOpen)
                {
                    CreateMenuButtons();
                }
                else
                {
                    yield return PlayOpenCloseMenu();
                }
            }

            yield return null;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public IEnumerator CloseMenu()
    {
        yield return PlayOpenCloseMenu();
    }

    private void DestroyMenu()
    {
        IsMenuOpen = false;
        foreach (var animate in leftAnimationList) Destroy(animate.transform.parent.gameObject);
        foreach (var animate in rightAnimationList) Destroy(animate.transform.parent.gameObject);
        leftAnimationList.Clear();
        rightAnimationList.Clear();
    }

    public void CreateMenuButtons()
    {
        IsCreateMenuOpen = true;
        DestroyMenu();
        AddButton("닫기", CloseMenu);
        if (SchedulerManager.IsRunScheduler)
        {
            AddButton("행동 멈춤", StopMove);
        }
        else
        {
            AddButton("행동 시작", StartMove);
        }
        AddButton("종료", Exit);
        StartCoroutine(PlayOpenCloseMenu());
    }

    private IEnumerator PlayOpenCloseMenu()
    {
        // Debug.Log(IsMenuOpen);
        if (!IsMenuOpen)
        {
            menuGameObject.SetActive(true);
            IsMenuOpen = true;
            foreach (var anime in leftAnimationList)
            {
                anime.clip = leftClip;
                var animationState = anime[anime.clip.name];
                animationState.time = 0;
                animationState.speed = 1;
                anime.Play();
            }

            foreach (var anime in rightAnimationList)
            {
                anime.clip = rightClip;
                var animationState = anime[anime.clip.name];
                animationState.time = 0;
                animationState.speed = 1;
                anime.Play();
            }
        }
        else
        {
            foreach (var anime in leftAnimationList)
            {
                anime.clip = leftClip;
                var animationState = anime[anime.clip.name];
                animationState.time = animationState.length;
                animationState.speed = -1;
                anime.Play();
            }

            foreach (var anime in rightAnimationList)
            {
                anime.clip = rightClip;
                var animationState = anime[anime.clip.name];
                animationState.time = animationState.length;
                animationState.speed = -1;
                anime.Play();
            }
            IsMenuOpen = false;
            IsCreateMenuOpen = false;
        }

        var animationList = new List<Animation>();
        animationList.AddRange(leftAnimationList);
        animationList.AddRange(rightAnimationList);


        yield return new WaitUntil(() => !animationList.Exists(data => data == null
                                                                       || data.isPlaying));

        if (!IsMenuOpen)
        {
            DestroyMenu();
        }
    }

    public void AddButton(string text, Func<IEnumerator> action)
    {
        var menu = Instantiate(menuButton, menuTransform).GetComponent<MenuButton>().SetButton(text, action);
        // Debug.Log($"left : {leftAnimationList.Count} <= right : {rightAnimationList.Count}");
        
        if (leftAnimationList.Count <= rightAnimationList.Count)
        {
            leftAnimationList.Add(menu.menuAnimation);
        }
        else
        {
            rightAnimationList.Add(menu.menuAnimation);
        }
    }
    
    public void AddMonitorButton(string text,int num ,Func<int , IEnumerator> action)
    {
        var menu = Instantiate(menuButton, menuTransform).GetComponent<MenuButton>().SetMonitorButton(text, num ,action);
        
        if (leftAnimationList.Count <= rightAnimationList.Count)
        {
            leftAnimationList.Add(menu.menuAnimation);
        }
        else
        {
            rightAnimationList.Add(menu.menuAnimation);
        }
    }
    
    public IEnumerator StartMove()
    {
        SchedulerManager.Instance.StartSchedule();
        SchedulerManager.IsRunScheduler = true;
        yield return PlayOpenCloseMenu();
    }

    public IEnumerator StopMove()
    {
        SchedulerManager.Instance.StopSchedule();
        SchedulerManager.IsRunScheduler = false;
        yield return PlayOpenCloseMenu(); 
    }

    public IEnumerator SetMonitor()
    {
        yield return PlayOpenCloseMenu();

        Debug.Log(Display.displays.Length);
        for (var i = 0; i < Display.displays.Length; i++)
        {
            var display = Display.displays[i];
            AddMonitorButton($"{i + 1} : {display.renderingWidth} || {display.renderingHeight}", i , CallBackSetMonitor);
        }
        IsCreateMenuOpen = true;
        yield return PlayOpenCloseMenu();
    }

    public IEnumerator CallBackSetMonitor(int num)
    {
        Debug.Log(num);
        TransparentApp.API.MoveWindowToMonitor(num);
        yield return PlayOpenCloseMenu();
    }

    public IEnumerator Exit()
    {
        Application.Quit();
        yield break;
    }
    
}
