using System;
using WinAPI.DNS;
using System.Text;
using RPC;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using BitDNS.Properties;

namespace BitDNS
{
    class Program
    {
        private static bool cont = true;

        static void Main(string[] args)
        {
            NotifyIcon NI = new NotifyIcon();
            NI.Icon = Resources.image_icon;
            NI.Visible = true;
            NI.Text = "Double click to exit";
            NI.DoubleClick += new EventHandler(NI_DoubleClick);
            waitForBM();

            XMLsrv x = new XMLsrv(8337);
            NI.ShowBalloonTip(10000, "BitDNS launched", "BitDNS has been started and will close itself with bitmessage when done", ToolTipIcon.Info);

            while (cont)
            {
                Process[] PP = Process.GetProcessesByName("bitmessage");
                if (PP == null || PP.Length == 0)
                {
                    cont = false;
                }
                else
                {
                    Application.DoEvents();
                    Thread.Sleep(500);
                    foreach (Process P in PP)
                    {
                        P.Dispose();
                    }
                }
            }
            NI.Visible = false;
        }

        private static void waitForBM()
        {
            bool wait = true;
            while (wait)
            {
                Process[] PP = Process.GetProcessesByName("bitmessage");
                if (PP == null || PP.Length == 0)
                {
                    foreach (Process P in PP)
                    {
                        P.Dispose();
                    }
                }
                else
                {
                    wait = false;
                }
            }
        }

        static void NI_DoubleClick(object sender, EventArgs e)
        {
            cont = false;
        }

        public static BMA[] getTXT(string DNS)
        {
            RecordCollection RC = null;
            try
            {
                RC = Dns.Lookup(DNS, WinAPI.DNS.Dns.QueryTypes.DNS_TYPE_TEXT);
            }
            catch
            {
                return null;
            }
            List<BMA> entries = new List<BMA>();
            foreach (object o in RC.SubRecords)
            {
                if (o is Dns.DNS_TXT_DATA)
                {
                    string[] ss=RecordCollection.ToString(((Dns.DNS_TXT_DATA)o).pStringArray).Split(';');
                    foreach (string s in ss)
                    {
                        if (s.Contains("BM-"))
                        {
                            if (s.Contains("="))
                            {
                                entries.Add(new BMA(s.Split('=')[0], s.Split('=')[1]));
                            }
                            else
                            {
                                entries.Add(new BMA(string.Empty, s));
                            }
                        }
                    }
                }
            }
            return entries.ToArray();
        }
    }
}
