using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Application = UnityEngine.Application;

public class MoveManager : MonoBehaviour
{
    public static MoveManager Instance;
    
    public RectTransform pickRectTransform;
    public Transform arisTransform;

    public static bool IsGround;
    public static bool IsPick;
    public static string NowDirection;
    private void Awake()
    {
        Instance = this;
    }
    
    private float _gravity = 2;
    private bool _isLeft;
    private float _rigidSpeed = 0.65f;
    private Vector2 _rigidBodyPos;
    
    private readonly Queue<(float x, float y)> _rigidBodyPosQueue = new(); 

    public void Update()
    {
        IsPick = Input.GetMouseButton(0);
        if (!MenuManager.IsMenuOpen && IsPick)
        {
            IsGround = false;
            StartCoroutine(SchedulerManager.Instance.Pick());
            _rigidBodyPos = GetAverageRigidPos();
            // Debug.Log(_rigidBodyPos);
            StartCoroutine(AddRigidPos());
            
            if (Application.isEditor)
            {
                return;
            }
            TransparentApp.API.Pick();
            _gravity = 2;
            return;
        }
        
        if (Application.isEditor) return;

        IsGround = TransparentApp.IsGround();
        
        if (!IsGround)
        {
            _gravity += Time.deltaTime * 30;
            // var moveDelta = 100f * Time.deltaTime;
            // _rigidBodyPos.x = moveDelta >= 0.8f ? _rigidBodyPos.x : _rigidBodyPos.x * moveDelta;
            // _rigidBodyPos.y = moveDelta >= 0.8f ? _rigidBodyPos.y : _rigidBodyPos.y * moveDelta;
            var rect = TransparentApp.GetLeftUpVector2();
            _rigidSpeed = _rigidSpeed >= 0.999f ? 0.999f : _rigidSpeed + 15 * Time.deltaTime;
            NowDirection = _rigidBodyPos.x > 0 ? "right" : "left";
            _rigidBodyPos.x *= _rigidSpeed;
            if (0 == (int)rect.y)
            {
                _rigidBodyPos.y = 0;
                _gravity = 2;
            }
            else
                _rigidBodyPos.y = _rigidBodyPos.y * _rigidSpeed;
            // Debug.Log($"{_rigidBodyPos} ||| Time : {moveDelta:F2}" );
            TransparentApp.API.Move((int)rect.x - (int)_rigidBodyPos.x,(int)rect.y - (int)_rigidBodyPos.y + (int)_gravity);
        }
        else
        {
            _rigidSpeed = 0.65f;
            _rigidBodyPosQueue.Clear();
            _gravity = 2;
        }
    }

    private IEnumerator AddRigidPos()
    {
        var (oldX, oldY) = TransparentApp.GetWindowsPos();
        yield return new WaitForSecondsRealtime(1 / 30f);
        var (newX, newY) = TransparentApp.GetWindowsPos();
        _rigidBodyPosQueue.Enqueue((oldX - newX, oldY - newY));
        if (_rigidBodyPosQueue.Count > 3)
        {
            _rigidBodyPosQueue.Dequeue();
        }
    }

    private Vector2 GetAverageRigidPos()
    {
        if (_rigidBodyPosQueue.Count == 0)
            return default;
        
        // Debug.Log(_rigidBodyPosQueue.Sum(data => data.x));
        var x = _rigidBodyPosQueue.Sum(data => data.x) / _rigidBodyPosQueue.Count;
        var y = _rigidBodyPosQueue.Sum(data => data.y) / _rigidBodyPosQueue.Count;

        x /= 5f * (Screen.mainWindowDisplayInfo.width / 1920f);
        y /= 5f * (Screen.mainWindowDisplayInfo.height / 1080f);
        
        // Debug.Log(Screen.mainWindowDisplayInfo.width);
        // Debug.Log((Screen.mainWindowDisplayInfo.width / 1920));
        return new Vector2(x, y);
    }
    
    public void SetRotate(int rotateY)
    {
        arisTransform.eulerAngles = new Vector3(0, rotateY, 0);
    }
}
