using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace BitDNS
{
    public static class AddressBook
    {
        private const string ADDRBOOK = "addr.txt";
        private const int LBL = 0;
        private const int ADDR = 1;

        public static BMA[] Addresses
        {
            get
            {
                if (File.Exists(ADDRBOOK))
                {
                    List<BMA> LA = new List<BMA>();
                    foreach (string s in File.ReadAllLines(ADDRBOOK))
                    {
                        if (s.Split('\t').Length >= 2)
                        {
                            LA.Add(new BMA(s.Split('\t')[LBL], s.Split('\t')[ADDR]));
                        }
                    }
                    return LA.ToArray();
                }
                return new BMA[0];
            }
            set
            {
                if (File.Exists(ADDRBOOK))
                {
                    File.Delete(ADDRBOOK);
                }
                if (value != null && value.Length > 0)
                {
                    string[] Lines = new string[value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                        Lines[i] = value[i].ToString();
                    }
                    File.WriteAllLines(ADDRBOOK, Lines);
                    Lines = null;
                }
            }
        }

        public static string Get(string L)
        {
            List<string> AA = new List<string>();
            BMA[] BA = Addresses;

            foreach (BMA A in BA)
            {
                if (A.Label.ToLower() == L)
                {
                    AA.Add(A.Address);
                }
            }

            if (AA.Count > 0)
            {
                return string.Join(";", AA.ToArray());
            }
            AA.Clear();
            AA = null;
            return null;
        }

        public static void Add(BMA a)
        {
            List<BMA> LA = new List<BMA>(Addresses);
            LA.Add(a);
            Addresses = LA.ToArray();
            LA.Clear();
            LA = null;
        }

        public static void RemoveAt(int i)
        {
            List<BMA> LA = new List<BMA>(Addresses);
            if (LA.Count > i && i >= 0)
            {
                LA.RemoveAt(i);
                Addresses = LA.ToArray();
                LA.Clear();
                LA = null;
            }
            else
            {
                throw new IndexOutOfRangeException("Index is too big or too small");
            }
        }

        public static void Remove(BMA a,bool all)
        {
            BMA[] BA=Addresses;
            for (int i = 0; i < BA.Length; i++)
            {
                if (BA[i].Equals(a))
                {
                    RemoveAt(i--);
                    if (!all)
                    {
                        break;
                    }
                }
            }
        }
    }
}
