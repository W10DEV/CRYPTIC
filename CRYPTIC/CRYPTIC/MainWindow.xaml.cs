using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CRYPTIC
{
    public partial class MainWindow : Window
    {
        private static DispatcherTimer UpdateUI = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            Update.Check();
            SystemTray.Setup();
            GUI.Load();
            SetupOverlay();
            InitiateHUD();
        }

        private void SetupOverlay()
        {
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.WindowState = WindowState.Maximized;
            this.Topmost = true;
            this.ShowInTaskbar = false;

            GUI.SetupUI(UIHolder);
            GUI.ApplyColorMatrixToImage(UIHolder);
            GUI.TextBlockSetup(TotalBounties, 12);
            GUI.TextBlockSetup(ShipsDestroyed, 12);
            GUI.TextBlockSetup(TargetType, 12);
            GUI.TextBlockSetup(TargetBounty, 12);
        }
        
        private void InitiateHUD()
        {
            Journals.Start();

            UpdateUI.Tick += UpdateUI_Tick;
            UpdateUI.Interval = new TimeSpan(0, 0, 0, 0, 50);
            UpdateUI.Start();
        }

        private void UpdateUI_Tick(object sender, EventArgs e)
        {
            TotalBounties.Text = CMDR.TotalBounties.ToString("N0") + " CR";
            TotalBounties.Foreground = new SolidColorBrush(GUI.Primary);
            TotalBounties.Effect = GUI.Glow(GUI.Primary);

            ShipsDestroyed.Text = CMDR.ShipsDestroyed.ToString() + " SHIPS";
            ShipsDestroyed.Foreground = new SolidColorBrush(GUI.Secondary);
            ShipsDestroyed.Effect = GUI.Glow(GUI.Secondary);

            switch (CMDR.TargetType)
            {
                case 0:
                    TargetType.Foreground = new SolidColorBrush(GUI.Friendly);
                    TargetType.Effect = GUI.Glow(GUI.Friendly);
                    TargetType.Text = "";
                    break;
                case 1:
                    TargetType.Foreground = new SolidColorBrush(GUI.Secondary);
                    TargetType.Effect = GUI.Glow(GUI.Secondary);
                    TargetType.Text = "SCANNING";
                    TargetType.FontSize = 32;
                    break;
                case 2:
                    TargetType.Foreground = new SolidColorBrush(GUI.Friendly);
                    TargetType.Effect = GUI.Glow(GUI.Friendly);
                    TargetType.Text = "FRIENDLY";
                    TargetType.FontSize = 36;
                    break;
                case 3:
                    TargetType.Foreground = new SolidColorBrush(GUI.Hostile);
                    TargetType.Effect = GUI.Glow(GUI.Hostile);
                    TargetType.Text = "HOSTILE";
                    TargetType.FontSize = 36;
                    break;
            }

            if (CMDR.TargetType == 3 && CMDR.TargetBounty > 0)
            {
                TargetBounty.Text = CMDR.TargetBounty.ToString("N0") + " CR";
                TargetBounty.Foreground = new SolidColorBrush(GUI.Hostile);
                TargetBounty.Effect = GUI.Glow(GUI.Hostile);
            } else
            {
                TargetBounty.Text = "";
            }
        }
    }
}
