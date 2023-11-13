using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuManager : MonoBehaviour
{
    [Header("MenuGameObject")] 
    public GameObject menuGameObject;

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
                yield return CloseMenuCoroutine();
            }

            yield return null;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public void CloseMenu()
    {
        StartCoroutine(CloseMenuCoroutine());
    }

    public IEnumerator CloseMenuCoroutine()
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
    
    public void StartMove()
    {
        SchedulerManager.Instance.StartSchedule();
    }

    public void StopMove()
    {
        SchedulerManager.Instance.StopSchedule();
    }

    public void Exit()
    {
        Application.Quit();
    }
    
}
