using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Script.Data;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
        public int left;
        public int top;
        public int right;
        public int bottom;
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

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool SetWindowText(int hwnd, String lpString);

    void Awake()
    {
        API = this;
        if (Application.isEditor) return;
#if UNITY_STANDALONE_WIN
        // var windowsName = $"Aris {Random.Range(float.MinValue, float.MaxValue)}";
        
        Screen.SetResolution(200,250,false);
        
        hWnd = (int)FindWindow(null, Application.productName);
        SetWindowText(hWnd, $"Aris {Random.Range(float.MinValue, float.MaxValue)}");
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
        SetLayeredWindowAttributes(hWnd, 0x000300, 255, LWA_ALPHA | LWA_COLORKEY);
        
        BringWindowToTop(hWnd);
        int style = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, (style & ~WS_BORDER & ~WS_CAPTION));
        
        int hMonitor = MonitorFromWindow(hWnd, 0);
        MONITORINFO monitorInfo = new MONITORINFO(); monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        GetMonitorInfo(hMonitor, ref monitorInfo);

        MoveWindowToBottomRight();
#endif
    }
    
    
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool GetWindowRect(int hWnd, out RECT lpRect);

    public static (int x, int y) GetWindowsPos()
    {
        if (GetWindowRect(hWnd, out var rect))
        {
            // int width = Rect.Right - Rect.Left;
            // int height = Rect.Bottom - Rect.Top;
            return (rect.Left, rect.Top);
        }
        throw new NullReferenceException();
    }


//     public void Update()
//     {
//         if (Application.isEditor) return;
//         if (GameManager.IsGameMode)
//         {
//             return;
//         }
// #if UNITY_STANDALONE_WIN
//         // int hMonitor = MonitorFromWindow(hWnd, 0);
//         // MONITORINFO monitorInfo = new MONITORINFO();
//         // monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
//         // GetMonitorInfo(hMonitor, ref monitorInfo);
//         
//         BringWindowToTop(hWnd);
//         int style = GetWindowLong(hWnd, GWL_STYLE);
//         SetWindowLong(hWnd, GWL_STYLE, (style & ~WS_BORDER & ~WS_CAPTION));
// #endif
//     }
    public void MoveWindowToBottomRight()
    {
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        RECT rect;
        if (GetWindowRect(hWnd, out rect))
        {
           
            int windowWidth = rect.Right - rect.Left;
            int windowHeight = rect.Bottom - rect.Top;
            
            int newX = screenWidth - windowWidth;
            int newY = screenHeight - windowHeight;
        
            Move(newX, newY);
        }
    }
    
    public static RECT GetTaskbarRect()
    {
        IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", null);
        
        if (taskbarHandle == IntPtr.Zero)
        {
            return default;
        }

        if (!GetWindowRect((int)taskbarHandle, out var rect) || rect.Top < Screen.mainWindowDisplayInfo.height / 2)
        {
            return default;
        }
        return rect;
    }

    public static RECT GetRect()
    {
        if (GetWindowRect(hWnd, out var rect))
        {
            return rect;
        }
        return default;
    }

    public static Vector2 GetLeftUpVector2()
    {
        if (GetWindowRect(hWnd, out var rect))
        {
            return new Vector2(rect.Left, rect.Top);
        }

        return default;
    }


    public void Move(int newX, int newY)
    {
#if UNITY_STANDALONE_WIN
        if (Application.isEditor) return;
        
        GetSafePos(newX, newY, out newX, out newY);
        
        SetWindowPos(hWnd, HWND_TOPMOST, newX, newY, 0, 0, SWP_NOSIZE);
#endif
    }

    private static void GetSafePos(int oldX , int oldY ,out int newX, out int newY)
    {
        if (oldX >= Screen.mainWindowDisplayInfo.width - Screen.width)
        {
            oldX = Screen.mainWindowDisplayInfo.width - Screen.width;
        }
        if (oldX <= 0)
        {
            oldX = 0;
        }

        if (oldY >= Screen.mainWindowDisplayInfo.height)
        {
            oldY = Screen.mainWindowDisplayInfo.height;
        }
            
        // 윈도우 작업 표시줄 불러오기
        var top = GetTaskbarRect().Top;
            
        // DebugUi.Debug = $"{top}";
        if (oldY > top - Screen.height)
        {
           oldY = top - Screen.height;
        }

        if (top == 0 && oldY >= Screen.mainWindowDisplayInfo.height - Screen.height)
        {
            oldY = Screen.mainWindowDisplayInfo.height - Screen.height;
        }

        if (oldY < 0)
        {
            oldY = 0;
        }
        
        newX = oldX;
        newY = oldY;
    }

    public static bool IsGround()
    {
        if (GetWindowRect(hWnd, out var rect))
        {
            return rect.Bottom >= GetTaskbarRect().Top;
        }
        return false;
    }

    public void Pick()
    {
        if (Application.isEditor) return;
        POINT p;
        
        if (GetCursorPos(out p))
        {
#if UNITY_STANDALONE_WIN
            // int hMonitor = MonitorFromWindow(hWnd, 0);
            // MONITORINFO monitorInfo = new MONITORINFO();
            // monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            // var t = GetMonitorInfo(hMonitor, ref monitorInfo);
            
            // 현재 게임 창의 크기
            int windowWidth = Screen.width;
            int windowHeight = Screen.height;
            
            // 게임 창을 마우스 위치에 중앙으로 옮기기
            int newX = p.x - windowWidth / 2;
            int newY = p.y - windowHeight / 2;

            var pos = MoveManager.Instance.pickRectTransform.localPosition;
            
            Move(newX - (int)pos.x,newY + (int)pos.y);
#endif
        }        
    }
    
    [DllImport("user32.dll")]
    public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lpRect, MonitorEnumProc callback, IntPtr dwData);

    // [DllImport("user32.dll", CharSet = CharSet.Auto)]
    // public static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

    public delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);
    
    public void MoveWindowToMonitor(int monitorIndex)
    {
#if UNITY_STANDALONE_WIN
        Debug.Log($"{Screen.mainWindowDisplayInfo.name}");
        var num = 0;
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, 
            (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData) =>
            {
                if (num == monitorIndex)
                {
                    Debug.Log($"Monitor Bounds: {lprcMonitor.Right} || {lprcMonitor.Top} || {lprcMonitor.Bottom} || {lprcMonitor.Left}");
                    MONITORINFO monitorInfo = new MONITORINFO();
                    GetMonitorInfo((int)hMonitor,ref monitorInfo);
                    SetWindowPos(hWnd, HWND_TOPMOST, lprcMonitor.Left, lprcMonitor.Top, 0, 0, SWP_NOSIZE);
                    // Screen.MoveMainWindowTo(Screen.mainWindowDisplayInfo, new Vector2Int(0, 0));
                    Screen.SetResolution(200, 250, false);
                }
                num++;
                return true;
                
            }, IntPtr.Zero);
#endif
    }
    
    // public void CreateMenu()
    // {
    //     
    // }
}

