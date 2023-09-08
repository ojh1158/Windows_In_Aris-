using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(int hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    public static extern int GetActiveWindow();

    // 윈도우를 이동시키기 위한 상수
    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_NOZORDER = 0x0004;

    private int hWnd;
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    // void Awake()
    // {
    //     Screen.SetResolution(500, 500, FullScreenMode.Windowed);
    //     // 유니티 게임의 윈도우 핸들을 가져옴
    //     hWnd = GetActiveWindow();
    // }
    //
    // public void Update()
    // {
    //     MoveScreen();
    // }
    //
    // void MoveScreen()
    // {
    //    SetWindowPos(hWnd, HWND_TOPMOST, 500, 500, 0, 0, SWP_NOSIZE);
    // }
}
