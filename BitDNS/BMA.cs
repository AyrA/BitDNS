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
    }
}
