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


    public static bool IsMenuOpen;

    public void Start()
    {
        StartCoroutine(WaitForRightClick());
    } 

    private IEnumerator WaitForRightClick()
    {
        while (true)
        {
            if (Input.GetMouseButton(1))
            {
                CreateMenuButtons();
            }

            yield return null;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public IEnumerator CloseMenu()
    {
        yield return PlayOpenCloseMenu();
    }

    public void CreateMenuButtons()
    {
        AddButton("닫기", CloseMenu);
        if (SchedulerManager.IsRunScheduler)
        {
            AddButton("행동 멈춤", StopMove);
        }
        else
        {
            AddButton("행동 시작", StartMove);
        }
        AddButton("모니터 선택", SetMonitor);
        AddButton("종료", Exit);
        
    }

    private IEnumerator PlayOpenCloseMenu()
    {
        if (!IsMenuOpen)
        {
            IsMenuOpen = true;
            menuGameObject.SetActive(true);
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
        }

        var animationList = new List<Animation>();
        animationList.AddRange(leftAnimationList);
        animationList.AddRange(rightAnimationList);


        yield return new WaitUntil(() => !animationList.Exists(data => data.isPlaying));

        if (!IsMenuOpen)
        {
            menuGameObject.SetActive(false);
        }
    }

    public void AddButton(string text, Func<IEnumerator> action)
    {
        var menu = Instantiate(menuButton, menuTransform).GetComponent<MenuButton>().SetButton(text, action);
        
        if (leftAnimationList.Count >= rightAnimationList.Count)
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
        
        if (leftAnimationList.Count >= rightAnimationList.Count)
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
        yield break;
    }

    public IEnumerator StopMove()
    {
        SchedulerManager.Instance.StopSchedule();
        yield break;
    }

    public IEnumerator SetMonitor()
    {
        yield return PlayOpenCloseMenu();

        for (var i = 0; i < Display.displays.Length; i++)
        {
            var display = Display.displays[i];
            AddMonitorButton($"{i} : {display.renderingWidth} || {display.renderingHeight}", i , CallBackSetMonitor);
        }

        yield return PlayOpenCloseMenu();
    }

    public IEnumerator CallBackSetMonitor(int num)
    {
        yield break;
    }

    public IEnumerator Exit()
    {
        Application.Quit();
        yield break;
    }
    
}
