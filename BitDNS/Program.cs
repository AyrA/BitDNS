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
            waitForBM();

            XMLsrv x = new XMLsrv(8337);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmAddressBook FA = new frmAddressBook();
            Thread t = new Thread(new ThreadStart(checkBM));
            t.IsBackground = true;
            t.Start();
            Application.Run();
            cont = false;
        }

        private static void checkBM()
        {
            while (cont)
            {
                Process[] PP = Process.GetProcessesByName("bitmessage");
                if (PP == null || PP.Length == 0)
                {
                    cont = false;
                }
                else
                {
                    Thread.Sleep(500);
                    foreach (Process P in PP)
                    {
                        P.Dispose();
                    }
                }
            }
            Application.Exit();
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
