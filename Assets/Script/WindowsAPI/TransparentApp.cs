using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class TransparentApp : MonoBehaviour
{
    public static TransparentApp API;
    
    [DllImport("user32.dll")]
    static extern int GetActiveWindow();

    [DllImport("user32.dll")]
    static extern int SetWindowLong(int hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    static extern bool SetLayeredWindowAttributes(int hWnd, uint crKey, byte bAlpha, uint dwFlags);
    
    [DllImport("user32.dll")]
     public static extern int BringWindowToTop(int hwnd);
     
     [DllImport("user32.dll")]
     [return: MarshalAs(UnmanagedType.Bool)]
     public static extern bool SetWindowPos(int hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
     
     [DllImport("user32.dll")]
     private static extern int GetWindowLong(int hWnd, int nIndex);
     
     const int GWL_EXSTYLE = -20;
    const int WS_EX_LAYERED = 0x80000;
    const int LWA_ALPHA = 0x2;
    const int LWA_COLORKEY = 0x1;
    
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    const UInt32 SWP_NOSIZE = 0x0001;
    
    const int GWL_STYLE = -16;
    const int WS_BORDER = 0x00800000;  // 경계선
    const int WS_CAPTION = 0x00C00000; // 타이틀 바
    
    [DllImport("user32.dll")]
    static extern int MonitorFromWindow(int hwnd, uint dwFlags);
    
    [DllImport("user32.dll")]
    static extern bool GetMonitorInfo(int hMonitor, ref MONITORINFO lpmi);
    
    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFO
    {
        public int cbSize;
        public Rect rcMonitor;
        public Rect rcWork;
        public uint dwFlags;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    
    private static int hWnd;
    
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }
    
    void Awake()
    {
        API = this;
        
        Screen.SetResolution(200,400,false);
        
        hWnd = GetActiveWindow();
        
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
        // SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE);
        SetLayeredWindowAttributes(hWnd, 0, 255, LWA_ALPHA | LWA_COLORKEY);
        
        BringWindowToTop(hWnd);
        int style = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, (style & ~WS_BORDER & ~WS_CAPTION));
        
        var pos = Screen.mainWindowDisplayInfo;
            
        SetWindowPos(hWnd, HWND_TOPMOST, (int)pos.width / 2, (int)pos.height / 2, 0, 0, SWP_NOSIZE);
        
        
        int hMonitor = MonitorFromWindow(hWnd, 0);
        MONITORINFO monitorInfo = new MONITORINFO();
        monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        GetMonitorInfo(hMonitor, ref monitorInfo);
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    public static Vector2 GetWindowsPos()
    {
        IntPtr findWindow = FindWindow(null, "Aris");  // 대상 윈도우의 타이틀을 입력하세요.

        if (findWindow != IntPtr.Zero)
        {
            RECT Rect;
            if (GetWindowRect(findWindow, out Rect))
            {
                int width = Rect.Right - Rect.Left;
                int height = Rect.Bottom - Rect.Top;
                
                return new Vector2(width, height);
            }
        }

        throw new NullReferenceException();
    }

    public void Update()
    {
        if (GameManager.IsGameMode)
        {
            return;
        }
        
        int hMonitor = MonitorFromWindow(hWnd, 0);
        MONITORINFO monitorInfo = new MONITORINFO();
        monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        GetMonitorInfo(hMonitor, ref monitorInfo);
        
        // var pos = GetWindowsPos();

        // var pos = Screen.mainWindowDisplayInfo;
        //     
        // SetWindowPos(hWnd, HWND_TOPMOST, (int)pos.width - pos.width / 2, (int)pos.height / 2, 0, 0, SWP_NOSIZE);

        var get = GetWindowsPos();
        
        DebugUi.Debug = get.x + "+" + get.y;
        
        BringWindowToTop(hWnd);
        int style = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, (style & ~WS_BORDER & ~WS_CAPTION));
    }

    public void Move(int newX, int newY)
    {
        // int hMonitor = MonitorFromWindow(hWnd, 0);
        // MONITORINFO monitorInfo = new MONITORINFO();
        // monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        // GetMonitorInfo(hMonitor, ref monitorInfo);
            
        // 현재 게임 창의 크기
        // int windowWidth = Screen.width;
        // int windowHeight = Screen.height;
        
        SetWindowPos(hWnd, HWND_TOPMOST, newX, newY, 0, 0, SWP_NOSIZE);
    }

    public void Pick()
    {
        POINT p;
        
        if (GetCursorPos(out p))
        {
            int hMonitor = MonitorFromWindow(hWnd, 0);
            MONITORINFO monitorInfo = new MONITORINFO();
            monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            GetMonitorInfo(hMonitor, ref monitorInfo);
            
            // // 현재 게임 창의 크기
            // int windowWidth = Screen.width;
            // int windowHeight = Screen.height;
            //
            // // 게임 창을 마우스 위치에 중앙으로 옮기기
            // int newX = p.x - windowWidth / 2;
            // int newY = p.y - windowHeight / 2;

            var pos = GetWindowsPos();
            
            Move((int)pos.x ,(int)pos.y + 50);
        }        
    }
}

