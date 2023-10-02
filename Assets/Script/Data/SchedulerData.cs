using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private static List<SchedulerData> _schedulerDataList;
        private static List<DialogueData> _dialogueDataList;
        
        private static Dictionary<ScheduleType, IEnumerator> scheduleDictionary = new()
        {
            { ScheduleType.Idle, Idle() },
            { ScheduleType.Walking, Walking() },
            { ScheduleType.Run, Run() },
            
        };

        public static void SetSchedulerData(List<SchedulerData> schedulerDataList)
        {
            _schedulerDataList = schedulerDataList;
            _dialogueDataList = DialogueManager.Instance.dialogueDataList.ToList();
            _dialogueDataList.RemoveAll(data => data.dialogueType == DialogueType.MemoReal);
        }

        public static Coroutine StartSchedule(ScheduleType scheduleType)
        {
            return SchedulerManager.Instance.StartCoroutine(scheduleDictionary.GetValueOrDefault(scheduleType));
        }
        
        private static IEnumerator Idle()
        {
            DebugUi.Debug = "Idle";
            DialogueManager.Instance.StartRandomWithType(_dialogueDataList[Random.Range(0, _dialogueDataList.Count)].dialogueType);
            SchedulerManager.Instance.animator.Play("idle");
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }

        private static readonly List<(string type,int x, int y)> MoveData = new()
        {
            ("left",1, 0),
            ("right",-1, 0)
        };

        private static IEnumerator Walking()
        {
            DebugUi.Debug = "Walking";
            (int x, int y) movePos = new();
            var move = MoveData[Random.Range(0, MoveData.Count)];

            float time = 0;
            while (time < Random.Range(5f, 10f))
            {
                var pos = TransparentApp.GetWindowsPos();
                if (movePos == pos)
                {
                    move = MoveData.Find(data => data.type != move.type);
                }
                movePos = pos;
                TransparentApp.API.Move(pos.x + move.x , pos.y + move.y);
                time += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private static IEnumerator Run()
        {
            DebugUi.Debug = "Run";
            yield break;
        }
        
    }
}