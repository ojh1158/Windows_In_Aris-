using System.Runtime.InteropServices;
using UnityEngine;


namespace Script.WindowsAPI
{
    public class SystemTray
    {
        public delegate void ResetCallBack(string message);

        [DllImport("SystemTray.dll")]
        public static extern void Reset(ResetCallBack callback);

        public void Init()
        {
            Reset(ResetGame);
        }

        private void ResetGame(string message)
        {
            
        }
    }
}