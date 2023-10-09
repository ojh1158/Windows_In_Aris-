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
        // Jump = 2,
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
    
    public class Schedule
    {
        private List<SchedulerData> _schedulerDataList;
        private List<DialogueData> _dialogueDataList;

        private List<(ScheduleType scheduleType, IEnumerator iEnumerator)> _scheduleList;

        // public Schedule()
        // {
        //     
        // }

        public void SetSchedulerData(List<SchedulerData> schedulerDataList)
        {
            _schedulerDataList = schedulerDataList;
            _dialogueDataList = DialogueManager.Instance.dialogueDataList.ToList();
            _dialogueDataList.RemoveAll(data => data.dialogueType == DialogueType.MemoReal);
        }

        public IEnumerator StartSchedule(ScheduleType scheduleType)
        { 
            _scheduleList = new()
            {
                ( ScheduleType.Idle, Idle()),
                ( ScheduleType.Walking, Walking())
            };
            return _scheduleList.Find(data => data.scheduleType == scheduleType).iEnumerator;
        }
        
        private IEnumerator Idle()
        {
            var startRandomWithType = DialogueManager.Instance.StartRandomWithType(_dialogueDataList[Random.Range(0, _dialogueDataList.Count)].dialogueType);
            SchedulerManager.Instance.animator.Play("idle");
            yield return DialogueManager.Instance.StartCoroutine(startRandomWithType);
            var shouldPlayEyesClose = Random.Range(0, 2) == 0;
            if (shouldPlayEyesClose)
            {
                yield return new WaitForSeconds(0.1f);
                DebugUi.Debug = "isClose";
                SchedulerManager.Instance.eyesClose.Play("EyesClose");
            }
            yield return new WaitForSeconds(Random.Range(3f, 6f));
            DebugUi.Debug = "";
        }

        private readonly List<(string type, int x, int y, int rotateY)> MoveData = new()
        {
            ("right",-1, 0, 0),
            ("left",1, 0, 180)
        };

        private IEnumerator Walking()
        {
            (int x, int y) movePos = new();
            var move = MoveData[Random.Range(0, MoveData.Count)];

            float time = 0;
            
            SchedulerManager.Instance.animator.Play("walk");
            
            MoveManager.In.SetRotate(move.rotateY);
            while (time < Random.Range(5f, 10f))
            {
                var pos = TransparentApp.GetWindowsPos();
                if (movePos == pos)
                {
                    move = MoveData.Find(data => data.type != move.type);
                    MoveManager.In.SetRotate(move.rotateY);
                }
                movePos = pos;
                TransparentApp.API.Move(pos.x + move.x , pos.y + move.y);
                time += 0.1f;
                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}