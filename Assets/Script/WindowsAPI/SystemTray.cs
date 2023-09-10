using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SystemTray : MonoBehaviour
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool Shell_NotifyIcon(uint dwMessage, ref NOTIFYICONDATA pnid);

    private const uint NIM_ADD = 0x00000000;
    private const uint NIM_MODIFY = 0x00000001;
    private const uint NIM_DELETE = 0x00000002;
    private const uint NIF_MESSAGE = 0x00000001;
    private const uint NIF_ICON = 0x00000002;
    private const uint NIF_TIP = 0x00000004;
    private const int WM_MOUSEMOVE = 0x0200;

    private NOTIFYICONDATA notifyIconData;

    private void Start()
    {
        CreateTrayIcon();
    }

    private void OnApplicationQuit()
    {
        RemoveTrayIcon();
    }

    private void CreateTrayIcon()
    {
        notifyIconData = new NOTIFYICONDATA();
        notifyIconData.cbSize = (uint)Marshal.SizeOf(notifyIconData);
        notifyIconData.uID = 0;
        notifyIconData.uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP;
        notifyIconData.hIcon = (IntPtr)LoadIcon(IntPtr.Zero, new IntPtr(32512)); // Load default icon
        notifyIconData.uCallbackMessage = WM_MOUSEMOVE;
        notifyIconData.szTip = "Unity Tray Icon";

        if (Shell_NotifyIcon(NIM_ADD, ref notifyIconData))
        {
            Debug.Log("Tray icon added.");
        }
        else
        {
            Debug.LogError("Failed to add tray icon.");
        }
    }

    private void RemoveTrayIcon()
    {
        if (Shell_NotifyIcon(NIM_DELETE, ref notifyIconData))
        {
            Debug.Log("Tray icon removed.");
        }
        else
        {
            Debug.LogError("Failed to remove tray icon.");
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NOTIFYICONDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uID;
        public uint uFlags;
        public uint uCallbackMessage;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);
}