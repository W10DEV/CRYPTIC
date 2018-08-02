using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CRYPTIC
{
    public static class Update
    {
        private static readonly float version = 1.0f;

        public static void Check()
        {
            string gitUpdate = "https://raw.githubusercontent.com/W10DEV/CRYPTIC/master/CRYPTIC/CRYPTIC/UpdateVersion/Update.txt";
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(gitUpdate);
            StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            float f = float.Parse(content);

            if(f > version)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Would you like to go to the Download Page?", "New Version Available!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://sites.google.com/view/elite-apps/");
                    Application.Current.Shutdown();
                    Environment.Exit(0);
                }
            }
        }
    }
}
