using System;
using System.Runtime.InteropServices;
using UnityEngine;


namespace Script.WindowsAPI
{
    public class SystemTray
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
            // RegisterCallback(delegate(string message) { DebugUi.Debug = message; });
        }

        public void Quit()
        {
            HideTrayIcon();
        }

        private void ResetGame(string message)
        {
            
        }
    }
}