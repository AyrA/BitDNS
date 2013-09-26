using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinAPI.DNS
{
    public class RecordCollection
    {
        private List<Dns.QueryTypes> Types;
        private List<object> Records;

        private IntPtr InitPtr;

        public Dns.DnsRecord InitRecord
        { get; private set; }

        public Dns.QueryTypes[] RecordTypes
        {
            get
            {
                return Types.ToArray();
            }
        }

        public object[] SubRecords
        {
            get
            {
                return Records.ToArray();
            }
        }

        public RecordCollection(IntPtr Ptr, Dns.DnsRecord R)
        {
            InitPtr = Ptr;
            InitRecord = R;
            Types = new List<Dns.QueryTypes>();
            Records = new List<object>();
        }

        ~RecordCollection()
        {
            Dns.DnsRecordListFree(InitPtr, Dns.DnsFreeType.DnsFreeFlat);
            Types.Clear();
            Records.Clear();
        }

        public static string ToString(IntPtr P)
        {
            return Marshal.PtrToStringAuto(P);
        }

        public void Add(Dns.QueryTypes rType, object Record)
        {
            Types.Add(rType);
            Records.Add(Record);
        }
    }
}
