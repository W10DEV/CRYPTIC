using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

namespace CRYPTIC
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            if (Properties.Settings.Default.JournalLocation.Length > 0)
            {
                JournalStatus.Content = "CUSTOM";
                JournalStatus.Foreground = new SolidColorBrush(Colors.Aqua);
                JournalStatusGlow.Color = Colors.Aqua;
            }

            if (Properties.Settings.Default.GraphicsLocation.Length > 0)
            {
                GraphicsStatus.Content = "CUSTOM";
                GraphicsStatus.Foreground = new SolidColorBrush(Colors.Aqua);
                GraphicsStatusGlow.Color = Colors.Aqua;
            }

            SaveBtn.Click += SaveBtn_Click;
            ClearBtn.Click += ClearBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
            JournalFind.Click += JournalFind_Click;
            GraphicsFind.Click += GraphicsFind_Click;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reset();
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        private void JournalFind_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            d.ShowDialog();
            if (VerifyFolder(d.SelectedPath))
                Properties.Settings.Default.JournalLocation = d.SelectedPath;
        }

        private void GraphicsFind_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            d.ShowDialog();
            if (VerifyFolder(d.SelectedPath))
                Properties.Settings.Default.GraphicsLocation = AppendGraphics(d.SelectedPath);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private string AppendGraphics(string path)
        {
            return path + "\\GraphicsConfigurationOverride.xml"; ;
        }

        private bool VerifyFolder(string path)
        {
            bool doesExist = false;
            if (Directory.Exists(path)) { doesExist = true; }
            return doesExist;
        }
    } 
}
