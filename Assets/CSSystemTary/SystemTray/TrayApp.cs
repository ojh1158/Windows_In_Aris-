using System;
using System.Drawing;
using System.Windows.Forms;

namespace SystemTray
{
    public class TrayApp : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        private TrayApp()
        {
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Exit", null, OnExit);

            
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Aris";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            
            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Visible = true;
            
            trayIcon.MouseDoubleClick += new MouseEventHandler(trayIcon_MouseDoubleClick);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;  // Hide form window.
            ShowInTaskbar = false;  // Remove from taskbar.
            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("더블 클릭 감지");
        }

        [STAThread]
        public static void Main()
        {
            Application.Run(new TrayApp());
        }
    }
}