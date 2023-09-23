using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArisScheduler : MonoBehaviour
{
    public List<SchedulerData> schedulerDataList;

    private Coroutine schedule;
    private void Awake()
    {
        StartCoroutine(Scheduler());
    }

    private IEnumerator Scheduler()
    {
        yield return new WaitForSeconds(Random.Range(7f, 15f));

        var num = Random.Range(0 ,schedulerDataList.Count);
        
        yield return RunSchedule();
    }

    private IEnumerator RunSchedule()
    {
        var scheduleType = (ScheduleType)Random.Range(0, Enum.GetValues(typeof(ScheduleType)).Length);
        yield return schedule = StartCoroutine(Schedule.StartSchedule(scheduleType));
        yield return new WaitForSeconds(Random.Range(5f, 10f));
    }
}
