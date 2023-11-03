using System;
using System.Runtime.InteropServices;
using UnityEngine;


namespace Script.WindowsAPI
{
    public struct SystemTray
    {
        public delegate void ResetCallBack(string message);
        
        [DllImport("libSystemTray.dll")]
        public static extern void ShowTrayIcon();

        [DllImport("libSystemTray.dll")]
        public static extern void HideTrayIcon();

        [DllImport("libSystemTray.dll")]
        public static extern void RegisterCallback(ResetCallBack callback);

        public void Init()
        {
            ShowTrayIcon();
        }

        public void Quit()
        {
            HideTrayIcon();
        }
    }
}