using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Script.Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class SchedulerManager : MonoBehaviour
{
    public static SchedulerManager Instance;
    
    [Header("Animator")]
    public Animator animator;
    public Animator eyesClose;
    
    [Header("Data")]
    public List<SchedulerData> schedulerDataList;

    private Coroutine _schedule;

    private Schedule schedule = new();
    
    private void Awake()
    {
        Instance = this;
        schedule.SetSchedulerData(schedulerDataList);
        StartCoroutine(RunSchedule());
    }

    public void StartScheduler(ScheduleType scheduleType)
    {
        StopCoroutine(_schedule);
        _schedule = StartCoroutine(schedule.StartSchedule(scheduleType));
        
    }

    private IEnumerator RunSchedule()
    {
        while (true)
        {
            var scheduleType = (ScheduleType)Random.Range(0, Enum.GetValues(typeof(ScheduleType)).Length);
            _schedule = StartCoroutine(schedule.StartSchedule(scheduleType));
            yield return _schedule;
            _schedule = null;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
