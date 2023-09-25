using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArisScheduler : MonoBehaviour
{
    [Header("Data")]
    public List<SchedulerData> schedulerDataList;

    private Coroutine schedule;
    private void Awake()
    {
        Schedule.SetSchedulerData(schedulerDataList);
        StartCoroutine(Scheduler());
    }

    private IEnumerator Scheduler()
    {
        yield return new WaitForSeconds(Random.Range(5f, 15f));
        yield return StartCoroutine(RunSchedule());
    }

    private IEnumerator RunSchedule()
    {
        var scheduleType = (ScheduleType)Random.Range(0, Enum.GetValues(typeof(ScheduleType)).Length);
        yield return schedule = StartCoroutine(Schedule.GetSchedule(scheduleType));
        schedule = null;
        StartCoroutine(Scheduler());
    }
}
