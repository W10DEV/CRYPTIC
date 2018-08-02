using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

//Use Journals.Read() to Initiate Thread and begin reading through Journals

namespace CRYPTIC
{

    public static class Journals
    {
        private static DispatcherTimer UpdateCMDR = new DispatcherTimer();
        private static readonly string _LOGS = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games\\Frontier Developments\\Elite Dangerous";
        private static Thread Thread;

        public static void Start()
        {
            UpdateCMDR.Tick += UpdateCMDR_Tick;
            UpdateCMDR.Interval = new TimeSpan(0, 0, 0, 0, 50);

            if (Directory.Exists(DIR()) == false)
            {
                MessageBox.Show("Select your Journals Directory and restart the Application.", "Journals Missing!");
                Stop();
                return;
            }

            UpdateCMDR.Start();
        }

        public static void Stop()
        {
            UpdateCMDR.Stop();
        }

        private static void UpdateCMDR_Tick(object sender, EventArgs e)
        {
            DirectoryInfo Dir = new DirectoryInfo(DIR());
            FileInfo[] Files = Dir.GetFiles("*.log");
            if (Files.Length <= 0)
                return;
            for (var i = 0; i < Files.Length; i++)
            {
                if (CMDR.fileIndex > i)
                    continue;
                if (i > CMDR.fileIndex)
                {
                    CMDR.fileIndex = i;
                    CMDR.lineIndex = -1;
                    CMDR.fileName = Files[i].Name;
                }

                FileStream fs = new FileStream(Files[i].FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);
                List<string> Lines = new List<string>();

                while (!sr.EndOfStream)
                {
                    Lines.Add(sr.ReadLine());
                }
                fs.Close();
                sr.Close();

                for (var x = 0; x < Lines.Count; x++)
                {
                    if (CMDR.lineIndex >= x)
                        continue;
                    if (x > CMDR.lineIndex)
                        CMDR.lineIndex = x;
                    ParseEntry(Lines[x]);
                }
            }
        }

        private static string DIR()
        {
            string pathToUse = _LOGS;
            if (Properties.Settings.Default.JournalLocation.Length > 0)
                pathToUse = Properties.Settings.Default.JournalLocation;
            return pathToUse;
        }

        private static void ParseEntry(string streamLine)
        {
            JObject obj = JObject.Parse(streamLine);

            if ((string)obj["event"] == "Died")
            {
                CMDR.TotalBounties = 0;
                CMDR.ShipsDestroyed = 0;
                CMDR.TargetBounty = 0;
                CMDR.TargetType = 0;
            }

            try
            {
                if ((string)obj["event"] == "Bounty" && obj["TotalReward"] != null)
                {
                    CMDR.TotalBounties = CMDR.TotalBounties + (int)obj["TotalReward"];
                    CMDR.ShipsDestroyed++;
                }
                if ((string)obj["event"] == "RedeemVoucher" && (string)obj["Type"] == "bounty")
                {
                    CMDR.TotalBounties = 0;
                    CMDR.ShipsDestroyed = 0;
                }

                //UPDATED CODE
                if ((string)obj["event"] == "ShipTargeted")
                {
                    if ((bool)obj["TargetLocked"])
                    {
                        if (obj["LegalStatus"] != null)
                        {
                            if((string)obj["LegalStatus"] == "Wanted")
                            {
                                CMDR.TargetType = 3;
                                if (obj["Bounty"] != null)
                                    CMDR.TargetBounty = (int)obj["Bounty"];
                            }
                            switch((string)obj["LegalStatus"])
                            {
                                case "Wanted":
                                    CMDR.TargetType = 3;
                                    if (obj["Bounty"] != null)
                                        CMDR.TargetBounty = (int)obj["Bounty"];
                                    break;
                                case "Clean":
                                    CMDR.TargetBounty = 0;
                                    CMDR.TargetType = 2;
                                    break;
                            }
                        }
                        else
                        {
                            CMDR.TargetType = 1;
                            CMDR.TargetBounty = 0;
                        }
                    }
                    else
                    {
                        CMDR.TargetType = 0;
                        CMDR.TargetBounty = 0;
                    }
                }

                if ((string)obj["event"] == "Shutdown")
                {
                    CMDR.TargetBounty = 0;
                    CMDR.TargetType = 0;
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(lineIndex + " - " + (string)obj["timestamp"]);
            }
        }
    }
}
