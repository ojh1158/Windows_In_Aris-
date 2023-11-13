using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Script.Data;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SchedulerManager : MonoBehaviour
{
    public static SchedulerManager Instance;

    [Header("GameObject")] 
    public RectTransform halo;
    
    [Header("Animator")]
    public Animator animator;
    public Animator eyesClose;
    public Animation haloAnimation;
    
    [Header("Data")]
    public List<SchedulerData> schedulerDataList;
    
    private Coroutine _schedule;
    private Schedule schedule = new();

    private bool _isPick;
    
    private void Awake()
    {
        Instance = this;
        schedule.SetSchedulerData(schedulerDataList);
        _schedule = StartCoroutine(RunSchedule());
    }

    public void StopSchedule()
    {
        StopCoroutine(_schedule);
        StopCoroutine(_runSchedule);
        DialogueManager.Instance.StopAllCoroutineAndIntoText();
        animator.Play("idle");
    }

    public void StartSchedule()
    {
        StopCoroutine(_schedule);
        StopCoroutine(_runSchedule);
        DialogueManager.Instance.StopAllCoroutineAndIntoText();
        _schedule = StartCoroutine(RunSchedule());
    }
    
    
    public IEnumerator Pick()
    {
        if (_isPick)
        {
            yield break;
        }
        _isPick = true;
        halo.transform.SetAsLastSibling();
        haloAnimation.Play("PickHalo");
        StopSchedule();
        yield return StartCoroutine(schedule.StartSchedule(ScheduleType.Pick));
        halo.transform.SetAsFirstSibling();
        haloAnimation.Play("halo");
        StartSchedule();
        _isPick = false;
    }

    private Coroutine _runSchedule;

    private readonly List<ScheduleType> _scheduleTypes = new()
    {
        ScheduleType.Idle,
        ScheduleType.Walking
    };

    private IEnumerator RunSchedule()
    {
        while (true)
        {
            // var scheduleType = (ScheduleType)Random.Range(0, Enum.GetValues(typeof(ScheduleType)).Length);
            var scheduleType = _scheduleTypes[Random.Range(0, _scheduleTypes.Count)];
            _runSchedule = StartCoroutine(schedule.StartSchedule(scheduleType));
            yield return _runSchedule;
            yield return new WaitForSeconds(0.1f);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
