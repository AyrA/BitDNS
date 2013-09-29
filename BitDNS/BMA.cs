using System;
using System.Collections.Generic;
using System.Text;

namespace BitDNS
{
    public class BMA
    {
        public string Label
        { get; set; }
        public string Address
        { get; set; }

        public BMA()
        {
        }

        public BMA(string L, string A)
        {
            Label = L;
            Address = A;
        }

        public override int GetHashCode()
        {
            return Label.GetHashCode() ^ Address.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{0}\t{1}", string.IsNullOrEmpty(Label) ? "" : Label, Address);
        }
        public override bool Equals(object obj)
        {
            if (obj is BMA)
            {
                return ((BMA)obj).Label == Label && ((BMA)obj).Address == Address;
            }
            return base.Equals(obj);
        }
    }
}
