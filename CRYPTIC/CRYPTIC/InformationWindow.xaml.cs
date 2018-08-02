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
using System.Windows.Threading;

namespace CRYPTIC
{
    public partial class InformationWindow : Window
    {
        DispatcherTimer UpdateUI = new DispatcherTimer();

        public InformationWindow()
        {
            InitializeComponent();
            CloseBtn.Click += CloseBtn_Click;
            UpdateUI.Tick += UpdateUI_Tick;
            UpdateUI.Interval = new TimeSpan(0, 0, 0, 2);
            UpdateUI.Start();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void UpdateUI_Tick(object sender, EventArgs e)
        {
            fileName.Text = CMDR.fileName;
            lineCount.Text = CMDR.lineIndex.ToString();
        }
    }
}
