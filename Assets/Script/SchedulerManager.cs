using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class SchedulerManager : MonoBehaviour
{
    public static SchedulerManager Instance;
    
    [Header("Animator")]
    public Animator animator;
    
    [Header("Data")]
    public List<SchedulerData> schedulerDataList;

    private Coroutine _schedule;
    
    
    
    private void Awake()
    {
        Instance = this;
        Schedule.SetSchedulerData(schedulerDataList);
        StartCoroutine(RunSchedule());
    }

    // public void StartScheduler(ScheduleType scheduleType)
    // {
    //     StopCoroutine(_schedule == null ? Scheduler() : RunSchedule());
    //     _schedule = StartCoroutine(Schedule.StartSchedule(scheduleType));
    // }

    // private IEnumerator Scheduler()
    // {
    //     DebugUi.Debug = "StartWait";
    //     StartCoroutine(RunSchedule());
    //     yield break;
    // }

    private IEnumerator RunSchedule()
    {
        // DebugUi.Debug = "pick";
        var scheduleType = (ScheduleType)Random.Range(0, Enum.GetValues(typeof(ScheduleType)).Length);
        DebugUi.Debug = scheduleType.ToString();
        _schedule = StartCoroutine(Schedule.StartSchedule(scheduleType));
        yield return _schedule; 
        StopCoroutine(_schedule);
        _schedule = null;
        // DebugUi.Debug = "ScheduleOk";
        StartCoroutine(RunSchedule());
    }
}
