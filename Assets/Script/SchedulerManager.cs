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
        StartCoroutine(Scheduler());
    }

    public void StartScheduler(ScheduleType scheduleType)
    {
        StopCoroutine(_schedule == null ? Scheduler() : RunSchedule());
        _schedule = Schedule.StartSchedule(scheduleType);
    }

    private IEnumerator Scheduler()
    {
        DebugUi.Debug = "StartWait";
        yield return StartCoroutine(RunSchedule());
    }

    private IEnumerator RunSchedule()
    {
        var scheduleType = (ScheduleType)Random.Range(0, Enum.GetValues(typeof(ScheduleType)).Length);
        yield return _schedule = Schedule.StartSchedule(scheduleType);
        _schedule = null;
        StartCoroutine(Scheduler());
    }
}
