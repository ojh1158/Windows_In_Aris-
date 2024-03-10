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
        Pick = 2,
        Fall = 3,
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
        private List<DialogueData> _dialogueIdleDataList;

        private List<(ScheduleType scheduleType, Func<IEnumerator> iEnumerator)> _scheduleList;

        public void SetSchedulerData()
        {
            _dialogueIdleDataList = DialogueManager.Instance.dialogueDataList.FindAll(data => data.dialogueType is DialogueType.Normal or DialogueType.Other);
        }

        public IEnumerator StartSchedule(ScheduleType scheduleType)
        { 
            _scheduleList = new()
            {
                ( ScheduleType.Idle, Idle),
                ( ScheduleType.Walking, Walking),
                ( ScheduleType.Pick, Pick),
                ( ScheduleType.Fall, Fall),
            };
            return _scheduleList.Find(data => data.scheduleType == scheduleType).iEnumerator();
        }
        
        private IEnumerator Idle()
        {
            var startRandomWithType = DialogueManager.Instance.StartRandomWithType(_dialogueIdleDataList[Random.Range(0, _dialogueIdleDataList.Count)].dialogueType);
            SchedulerManager.Instance.animator.Play("idle");
            yield return DialogueManager.Instance.StartCoroutine(startRandomWithType);
            var shouldPlayEyesClose = Random.Range(0, 2) == 0;
            if (shouldPlayEyesClose)
            {
                SchedulerManager.Instance.animator.Play("EyesClose");
                foreach (var animationClip in SchedulerManager.Instance.animator.runtimeAnimatorController
                             .animationClips)
                {
                    if (animationClip.name == "EyesClose")
                    {
                        yield return new WaitForSeconds(animationClip.length);
                    }
                }
            }
            yield return new WaitForSeconds(Random.Range(1f, 2f));
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
            
            MoveManager.Instance.SetRotate(move.rotateY);
            while (time < Random.Range(5f, 10f))
            {
                var pos = TransparentApp.GetWindowsPos();
                if (movePos == pos)
                {
                    move = MoveData.Find(data => data.type != move.type);
                    MoveManager.Instance.SetRotate(move.rotateY);
                }
                movePos = pos;
                TransparentApp.API.Move(pos.x + move.x , pos.y + move.y);
                time += 0.025f;
                yield return new WaitForSeconds(0.025f);
            }
        }

        private IEnumerator Pick()
        {
            SchedulerManager.Instance.animator.Play("Pick"); 
            DialogueManager.Instance.StartCoroutine(DialogueManager.Instance.StartRandomWithType(DialogueType.PickUp));
            yield return new WaitUntil(() => !MoveManager.IsPick);
            DialogueManager.Instance.StopAllCoroutineAndIntoText();
        }
        
        private IEnumerator Fall()
        {
            SchedulerManager.Instance.animator.Play("Fall");
            MoveManager.Instance.SetRotate(MoveData.Find(data => data.type == MoveManager.NowDirection).rotateY);
            while (true)
            {
                if (MoveManager.IsPick)
                {
                    SchedulerManager.Instance.StopSchedule();
                    SchedulerManager.Instance.StartCoroutine(SchedulerManager.Instance.Pick());
                    yield break;
                }
                if (MoveManager.IsGround)
                {
                    break;
                }
                yield return null;
            }
        }
    }
}