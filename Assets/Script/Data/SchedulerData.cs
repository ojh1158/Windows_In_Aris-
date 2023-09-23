using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Data
{
    public enum ScheduleType
    {
        Idle = 0,
        Walking = 1,
        Run = 2
    }

    [Serializable]
    public class SchedulerData
    {
        public ScheduleType scheduleType;
        public float maxTime;
        public float minTime;

        public (float Max, float Min) GetTime()
        {
            return (maxTime, minTime);
        }
    }

    public static class Schedule
    {
        private static Dictionary<ScheduleType, IEnumerator> scheduleDictionary = new()
        {
            {ScheduleType.Idle , Idle()},
            {ScheduleType.Walking , Walking()},
            { ScheduleType.Run, Run()}
        };

        public static IEnumerator StartSchedule(ScheduleType scheduleType)
        {
            return scheduleDictionary.GetValueOrDefault(scheduleType);
        }
        
        private static IEnumerator Idle()
        {
            //DialogueManager.Instance.Dialogue();
            yield break;
        }

        private static IEnumerator Walking()
        {
            yield break;
        }

        private static IEnumerator Run()
        {
            yield break;
        }
        
    }
}