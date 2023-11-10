using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace Script.WindowsAPI
{
    public struct SystemTray
    {
        [DllImport("SystemTrayClass", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetMonitorInfo(string[] monitorName);

        [DllImport("SystemTrayClass", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HideTrayIcon();

        [DllImport("SystemTrayClass", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Main();
        
        [DllImport("SystemTrayClass", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrayExit(object sender, EventArgs e);
        
        // [DllImport("TrayPlugin", CallingConvention = CallingConvention.Cdecl)]
        // public static extern void Exit();

        private static List<string> _monitorName = new();

        public SystemTray Start()
        {
            Main();
            return this;
        }

        public SystemTray SetMonitor()
        {
            for (int i = 0; i < Display.displays.Length; i++)
            {
                _monitorName.Add(i + " : " + Display.displays[i].systemWidth + "x" + Display.displays[i].systemHeight);
            }
            
            SetMonitorInfo(_monitorName.ToArray());
            return this;
        }

        public static void Exit()
        {
            TrayExit(null, null);
            Application.Quit();
        }
    }
}