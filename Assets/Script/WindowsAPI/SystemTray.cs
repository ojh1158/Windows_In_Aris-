using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine;
using Application = UnityEngine.Application;

namespace Script.WindowsAPI
{
//     public struct SystemTray
//     {
//         // [DllImport("Tray.dll", CallingConvention = CallingConvention.Cdecl)]
//         // private static extern void SetMonitorInfo(string[] monitorName);
//         //
//         // [DllImport("Tray", CallingConvention = CallingConvention.Cdecl)]
//         // public static extern void HideTrayIcon();
//         //
//         [DllImport("Tray", CallingConvention = CallingConvention.Cdecl)]
//         public static extern void Main();
//         //
//         // [DllImport("Tray", CallingConvention = CallingConvention.Cdecl)]
//         // public static extern void TrayExit(object sender, EventArgs e);
//         
//         // [DllImport("TrayPlugin", CallingConvention = CallingConvention.Cdecl)]
//         // public static extern void Exit();
//         
//
//         private static List<string> _monitorName = new();
//
//         private static NotifyIcon trayIcon;
//         private static ContextMenuStrip trayMenu;
//         public SystemTray Start()
//         {
//             Main();
//             
//             trayMenu = new ContextMenuStrip();
//             trayMenu.Items.Add("Exit", null, TrayExit);
//
//             
//             trayIcon = new NotifyIcon();
//             trayIcon.Text = "Aris";
//             trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
//             
//             trayIcon.ContextMenuStrip = trayMenu;
//             trayIcon.Visible = true;
// // #if UNITY_STANDALONE_WIN
// //             Tray.TrayPlugin.Main();
// // #endif
//             return this;
//         }
//
//         public SystemTray SetMonitor()
//         {
//             for (int i = 0; i < Display.displays.Length; i++)
//             {
//                 _monitorName.Add(i + " : " + Display.displays[i].systemWidth + "x" + Display.displays[i].systemHeight);
//             }
//             
//             // Tray.TrayPlugin.SetMonitorInfo(_monitorName.ToArray());
//             return this;
//         }
//
//         public void TEST()
//         {
//             
//         }
//
//         public static void Exit()
//         {
//             // Tray.TrayPlugin.TrayExit(null, null);
//             Application.Quit();
//         }
//     }
}
