// using System.Runtime.InteropServices;
// using UnityEngine;
//
// public class AlwaysOnTop : MonoBehaviour
// {
//     [DllImport("user32.dll")]
//     static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
//
//     [DllImport("user32.dll")]
//     static extern int GetActiveWindow();
//
//     const int HWND_TOPMOST = -1;
//     const uint SWP_NOMOVE = 0x0002;
//     const uint SWP_NOSIZE = 0x0001;
//
//     void Update()
//     {
//         int hWnd = GetActiveWindow();
//         SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
//     }
// }
