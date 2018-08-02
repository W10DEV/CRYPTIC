using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CRYPTIC
{
    public static class SystemTray
    {
        private static TaskbarIcon taskbarIcon = new TaskbarIcon();
        private static SettingsWindow Settings = new SettingsWindow();
        private static InformationWindow Information = new InformationWindow();

        public static void Setup()
        {
            taskbarIcon.Icon = CRYPTIC.Properties.Resources.icon;
            taskbarIcon.ToolTipText = "Watcher";

            ContextMenu contextMenu = new ContextMenu();

            MenuItem OpenSettings = new MenuItem();
            OpenSettings.Header = "Settings";
            OpenSettings.Click += OpenSettings_Click;
            MenuItem OpenInformation = new MenuItem();
            OpenInformation.Header = "Information";
            OpenInformation.Click += OpenInformation_Click;
            MenuItem ExitApplication = new MenuItem();
            ExitApplication.Header = "Exit";
            ExitApplication.Click += ExitApplication_Click;

            contextMenu.Items.Add(OpenSettings);
            contextMenu.Items.Add(OpenInformation);
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(ExitApplication);
            taskbarIcon.ContextMenu = contextMenu;
        }

        private static void OpenInformation_Click(object sender, RoutedEventArgs e)
        {
            Information.Show();
        }

        private static void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings.Show();
        }

        private static void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
    }
}
