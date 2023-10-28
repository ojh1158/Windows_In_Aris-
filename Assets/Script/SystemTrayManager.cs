using System;
using System.Collections;
using System.Collections.Generic;
using Script.WindowsAPI;
using UnityEngine;

public class SystemTrayManager : MonoBehaviour
{
   private void Awake()
   {
      new SystemTray().Init();
   }

   private void OnApplicationQuit()
   {
      new SystemTray().Quit();
   }
}
