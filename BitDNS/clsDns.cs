using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Net;

namespace WinAPI.DNS
{

    public static class Dns
    {
        [DllImport("dnsapi", EntryPoint = "DnsQuery_W", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        private static extern int DnsQuery([MarshalAs(UnmanagedType.VBByRefStr)]ref string pszName, QueryTypes wType, QueryOptions options, int aipServers, ref IntPtr ppQueryResults, int pReserved);
        
        [DllImport("dnsapi", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern void DnsRecordListFree(IntPtr pRecordList, DnsFreeType FreeType);

        [Flags]
        private enum QueryOptions : int
        {
            DNS_QUERY_ACCEPT_TRUNCATED_RESPONSE = 1,
            DNS_QUERY_BYPASS_CACHE = 8,
            DNS_QUERY_DONT_RESET_TTL_VALUES = 0x100000,
            DNS_QUERY_NO_HOSTS_FILE = 0x40,
            DNS_QUERY_NO_LOCAL_NAME = 0x20,
            DNS_QUERY_NO_NETBT = 0x80,
            DNS_QUERY_NO_RECURSION = 4,
            DNS_QUERY_NO_WIRE_QUERY = 0x10,
            DNS_QUERY_RESERVED = -16777216,
            DNS_QUERY_RETURN_MESSAGE = 0x200,
            DNS_QUERY_STANDARD = 0,
            DNS_QUERY_TREAT_AS_FQDN = 0x1000,
            DNS_QUERY_USE_TCP_ONLY = 2,
            DNS_QUERY_WIRE_ONLY = 0x100
        }

        public enum QueryTypes : int
        {
            DNS_TYPE_A = 0x0001,
            DNS_TYPE_NS = 0x0002,
            DNS_TYPE_MD = 0x0003,
            DNS_TYPE_MF = 0x0004,
            DNS_TYPE_CNAME = 0x0005,
            DNS_TYPE_SOA = 0x0006,
            DNS_TYPE_MB = 0x0007,
            DNS_TYPE_MG = 0x0008,
            DNS_TYPE_MR = 0x0009,
            DNS_TYPE_NULL = 0x000a,
            DNS_TYPE_WKS = 0x000b,
            DNS_TYPE_PTR = 0x000c,
            DNS_TYPE_HINFO = 0x000d,
            DNS_TYPE_MINFO = 0x000e,
            DNS_TYPE_MX = 0x000f,
            DNS_TYPE_TEXT = 0x0010,
            DNS_TYPE_RP = 0x0011,
            DNS_TYPE_AFSDB = 0x0012,
            DNS_TYPE_X25 = 0x0013,
            DNS_TYPE_ISDN = 0x0014,
            DNS_TYPE_RT = 0x0015,
            DNS_TYPE_NSAP = 0x0016,
            DNS_TYPE_NSAPPTR = 0x0017,
            DNS_TYPE_SIG = 0x0018,
            DNS_TYPE_KEY = 0x0019,
            DNS_TYPE_PX = 0x001a,
            DNS_TYPE_GPOS = 0x001b,
            DNS_TYPE_AAAA = 0x001c,
            DNS_TYPE_LOC = 0x001d,
            DNS_TYPE_NXT = 0x001e,
            DNS_TYPE_EID = 0x001f,
            DNS_TYPE_NIMLOC = 0x0020,
            DNS_TYPE_SRV = 0x0021,
            DNS_TYPE_ATMA = 0x0022,
            DNS_TYPE_NAPTR = 0x0023,
            DNS_TYPE_KX = 0x0024,
            DNS_TYPE_CERT = 0x0025,
            DNS_TYPE_A6 = 0x0026,
            DNS_TYPE_DNAME = 0x0027,
            DNS_TYPE_SINK = 0x0028,
            DNS_TYPE_OPT = 0x0029,
            DNS_TYPE_DS = 0x002B,
            DNS_TYPE_RRSIG = 0x002E,
            DNS_TYPE_NSEC = 0x002F,
            DNS_TYPE_DNSKEY = 0x0030,
            DNS_TYPE_DHCID = 0x0031,
            DNS_TYPE_UINFO = 0x0064,
            DNS_TYPE_UID = 0x0065,
            DNS_TYPE_GID = 0x0066,
            DNS_TYPE_UNSPEC = 0x0067,
            DNS_TYPE_ADDRS = 0x00f8,
            DNS_TYPE_TKEY = 0x00f9,
            DNS_TYPE_TSIG = 0x00fa,
            DNS_TYPE_IXFR = 0x00fb,
            DNS_TYPE_AXFR = 0x00fc,
            DNS_TYPE_MAILB = 0x00fd,
            DNS_TYPE_MAILA = 0x00fe,
            DNS_TYPE_ALL = 0x00ff,
            DNS_TYPE_ANY = 0x00ff,
            DNS_TYPE_WINS = 0xff01,
            DNS_TYPE_WINSR = 0xff02,
            DNS_TYPE_NBSTAT = DNS_TYPE_WINSR
        }

        internal enum DnsFreeType : int
        {
            DnsFreeFlat = 0,
            DnsFreeRecordList = 1,
            DnsFreeParsedMessageFields = 2
        }

        public static RecordCollection Lookup(string DnsName,QueryTypes LookupType)
        {
            IntPtr ptr=IntPtr.Zero;
            IntPtr temp=IntPtr.Zero;
            RecordCollection RC=null;
            DnsRecord DNSR;

            int intAddress = BitConverter.ToInt32(IPAddress.Parse("8.8.8.8").GetAddressBytes(), 0);
            int result=DnsQuery(ref DnsName,
                LookupType,
                QueryOptions.DNS_QUERY_STANDARD,
                0,ref ptr, 0);

            if (result == 0)
            {
                for (temp = ptr; !temp.Equals(IntPtr.Zero); temp = DNSR.pNext)
                {
                    DNSR = (DnsRecord)Marshal.PtrToStructure(temp, typeof(DnsRecord));
                    if(RC==null)
                    {
                        RC=new RecordCollection(temp,DNSR);
                    }
                    RC.Add((QueryTypes)DNSR.wType, getRecord((QueryTypes)DNSR.wType,temp));
                }
            }
            else
            {
                throw new Win32Exception(result);
            }
            return RC;
        }

        private static object getRecord(QueryTypes t, IntPtr RecordPtr)
        {
            IntPtr Record = (IntPtr)(RecordPtr.ToInt32() + 16+(IntPtr.Size*2));
            switch (t)
            {
                case QueryTypes.DNS_TYPE_A:
                    return Marshal.PtrToStructure(Record, typeof(DNS_A_DATA));
                case QueryTypes.DNS_TYPE_NS:
                case QueryTypes.DNS_TYPE_MD:
                case QueryTypes.DNS_TYPE_MF:
                case QueryTypes.DNS_TYPE_CNAME:
                case QueryTypes.DNS_TYPE_MB:
                case QueryTypes.DNS_TYPE_MG:
                case QueryTypes.DNS_TYPE_MR:
                case QueryTypes.DNS_TYPE_PTR:
                case QueryTypes.DNS_TYPE_DNAME:
                    return Marshal.PtrToStructure(Record, typeof(DNS_PTR_DATA));
                case QueryTypes.DNS_TYPE_HINFO:
                case QueryTypes.DNS_TYPE_TEXT:
                case QueryTypes.DNS_TYPE_X25:
                case QueryTypes.DNS_TYPE_ISDN:
                    return Marshal.PtrToStructure(Record, typeof(DNS_TXT_DATA));
                case QueryTypes.DNS_TYPE_AFSDB:
                case QueryTypes.DNS_TYPE_RT:
                case QueryTypes.DNS_TYPE_MX:
                case QueryTypes.DNS_TYPE_RP:
                    return Marshal.PtrToStructure(Record, typeof(DNS_MX_DATA));
                case QueryTypes.DNS_TYPE_MINFO:
                    return Marshal.PtrToStructure(Record, typeof(DNS_MINFO_DATA));
                case QueryTypes.DNS_TYPE_SOA:
                    return Marshal.PtrToStructure(Record, typeof(DNS_SOA_DATA));
                case QueryTypes.DNS_TYPE_NULL:
                    return Marshal.PtrToStructure(Record, typeof(DNS_NULL_DATA));
                case QueryTypes.DNS_TYPE_WKS:
                    return Marshal.PtrToStructure(Record, typeof(DNS_WKS_DATA));
                case QueryTypes.DNS_TYPE_SIG:
                    return Marshal.PtrToStructure(Record, typeof(DNS_SIG_DATA));
                case QueryTypes.DNS_TYPE_KEY:
                    return Marshal.PtrToStructure(Record, typeof(DNS_KEY_DATA));
                case QueryTypes.DNS_TYPE_AAAA:
                    return Marshal.PtrToStructure(Record, typeof(DNS_AAAA_DATA));
                case QueryTypes.DNS_TYPE_NXT:
                    return Marshal.PtrToStructure(Record, typeof(DNS_NXT_DATA));
                case QueryTypes.DNS_TYPE_SRV:
                    return Marshal.PtrToStructure(Record, typeof(DNS_SRV_DATA));
                case QueryTypes.DNS_TYPE_ATMA:
                    return Marshal.PtrToStructure(Record, typeof(DNS_ATMA_DATA));
                case QueryTypes.DNS_TYPE_NAPTR:
                    return Marshal.PtrToStructure(Record, typeof(DNS_NAPTR_DATA));
                case QueryTypes.DNS_TYPE_OPT:
                    return Marshal.PtrToStructure(Record, typeof(DNS_OPT_DATA));
                case QueryTypes.DNS_TYPE_DS:
                    return Marshal.PtrToStructure(Record, typeof(DNS_DS_DATA));
                case QueryTypes.DNS_TYPE_RRSIG:
                    return Marshal.PtrToStructure(Record, typeof(DNS_RRSIG_DATA));
                case QueryTypes.DNS_TYPE_NSEC:
                    return Marshal.PtrToStructure(Record, typeof(DNS_NSEC_DATA));
                case QueryTypes.DNS_TYPE_DNSKEY:
                    return Marshal.PtrToStructure(Record, typeof(DNS_DNSKEY_DATA));
                case QueryTypes.DNS_TYPE_DHCID:
                    return Marshal.PtrToStructure(Record, typeof(DNS_DHCID_DATA));
                case QueryTypes.DNS_TYPE_TKEY:
                    return Marshal.PtrToStructure(Record, typeof(DNS_TKEY_DATA));
                case QueryTypes.DNS_TYPE_TSIG:
                    return Marshal.PtrToStructure(Record, typeof(DNS_TSIG_DATA));
                case QueryTypes.DNS_TYPE_WINS:
                case QueryTypes.DNS_TYPE_WINSR:
                    return Marshal.PtrToStructure(Record, typeof(DNS_WINS_DATA));
                case QueryTypes.DNS_TYPE_UINFO:
                case QueryTypes.DNS_TYPE_UID:
                case QueryTypes.DNS_TYPE_GID:
                case QueryTypes.DNS_TYPE_UNSPEC:
                case QueryTypes.DNS_TYPE_ADDRS:
                case QueryTypes.DNS_TYPE_IXFR:
                case QueryTypes.DNS_TYPE_AXFR:
                case QueryTypes.DNS_TYPE_MAILB:
                case QueryTypes.DNS_TYPE_MAILA:
                case QueryTypes.DNS_TYPE_ALL:
                case QueryTypes.DNS_TYPE_SINK:
                case QueryTypes.DNS_TYPE_KX:
                case QueryTypes.DNS_TYPE_CERT:
                case QueryTypes.DNS_TYPE_A6:
                case QueryTypes.DNS_TYPE_EID:
                case QueryTypes.DNS_TYPE_NIMLOC:
                case QueryTypes.DNS_TYPE_LOC:
                case QueryTypes.DNS_TYPE_PX:
                case QueryTypes.DNS_TYPE_GPOS:
                case QueryTypes.DNS_TYPE_NSAP:
                case QueryTypes.DNS_TYPE_NSAPPTR:
                    break;
            }
            throw new Exception(string.Format("Unsupported Record type: {0}", t));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DnsRecord
        {
            public IntPtr pNext;       //0
            public IntPtr pName;       //4
            public short wType;        //8
            public short wDataLength;  //10
            public uint flags;         //12
            public uint dwTtl;         //16
            public uint dwReserved;    //20
            public IntPtr Record;      //24
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_A_DATA
        {
            public uint IpAddress;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_SOA_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNamePrimaryServer;
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameAdministrator;
            public uint dwSerialNo;
            public uint dwRefresh;
            public uint dwRetry;
            public uint dwExpire;
            public uint dwDefaultTtl;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_PTR_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameHost;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_MINFO_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameMailbox;
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameErrorsMailbox;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_MX_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameExchange;
            public ushort wPreference;
            public ushort Pad;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_TXT_DATA
        {
            public uint dwStringCount;
            /// <summary>
            /// Array of pointers to strings with dwStringCount elements
            /// </summary>
            public IntPtr pStringArray;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_NULL_DATA
        {
            public uint dwByteCount;
            public IntPtr Data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_WKS_DATA
        {
            public uint IpAddress;
            public byte chProtocol;
            public IntPtr BitMask;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_AAAA_DATA
        {
            public IP6_ADDRESS Ip6Address;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_KEY_DATA
        {
            public uint wFlags;
            public byte chProtocol;
            public byte chAlgorithm;
            public IntPtr Key;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_SIG_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameSigner;
            public uint wTypeCovered;
            public byte chAlgorithm;
            public byte chLabelCount;
            public uint dwOriginalTtl;
            public uint dwExpiration;
            public uint dwTimeSigned;
            public short wKeyTag;
            public short Pad;
            /// <summary>
            /// Base64 encoded. Probably 0-Terminated
            /// </summary>
            public IntPtr Signature;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_ATMA_DATA
        {
            public byte AddressType;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.U1)]
            public byte[] Address;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_NXT_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameNext;
            public ushort wNumTypes;
            /// <summary>
            /// Array of ushort (WORD) with length wNumTypes (2-8)
            /// </summary>
            public IntPtr wTypes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_SRV_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameTarget;
            public short wPriority;
            public short wWeight;
            public short wPort;
            public short Pad;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_NAPTR_DATA
        {
            public ushort wOrder;
            public ushort wPreference;
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pFlags;
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pService;
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pRegularExpression;
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pReplacement;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_OPT_DATA
        {
            public ushort wDataLength;
            public ushort wPad;
            /// <summary>
            /// Byte array of Length wDataLength
            /// </summary>
            public IntPtr Data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_DS_DATA
        {
            public ushort wKeyTag;
            public byte chAlgorithm;
            public byte chDigestType;
            public ushort wDigestLength;
            public ushort wPad;
            /// <summary>
            /// byte array of length wDigestLength
            /// </summary>
            public IntPtr Digest;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_RRSIG_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameSigner;
            public ushort wTypeCovered;
            public byte chAlgorithm;
            public byte chLabelCount;
            public uint dwOriginalTtl;
            public uint dwExpiration;
            public uint dwTimeSigned;
            public ushort wKeyTag;
            public ushort Pad;
            /// <summary>
            /// Length probably depends on chAlgorithm
            /// </summary>
            public IntPtr Signature;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_NSEC_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNextDomainName;
            public ushort wTypeBitMapsLength;
            public ushort wPad;
            /// <summary>
            /// Byte array with Length wTypeBitMapsLength
            /// </summary>
            public IntPtr TypeBitMaps;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_DNSKEY_DATA
        {
            public ushort wFlags;
            public byte chProtocol;
            public byte chAlgorithm;
            public ushort wKeyLength;
            public ushort wPad;
            /// <summary>
            /// Byte array with length wKeyLength
            /// </summary>
            public IntPtr Key;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_TKEY_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameAlgorithm;
            /// <summary>
            /// Points to a byte
            /// </summary>
            public IntPtr pAlgorithmPacket;
            /// <summary>
            /// Points to a byte
            /// </summary>
            public IntPtr pKey;
            /// <summary>
            /// Points to a byte
            /// </summary>
            public IntPtr pOtherData;
            public uint dwCreateTime;
            public uint dwExpireTime;
            public ushort wMode;
            public ushort wError;
            public ushort wKeyLength;
            public ushort wOtherLength;
            public byte cAlgNameLength;
            [MarshalAsAttribute(UnmanagedType.VariantBool)]
            public int bPacketPointers;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_TSIG_DATA
        {
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameAlgorithm;
            /// <summary>
            /// Points to a byte
            /// </summary>
            public IntPtr pAlgorithmPacket;
            /// <summary>
            /// Points to a byte
            /// </summary>
            public IntPtr pSignature;
            /// <summary>
            /// Points to a byte
            /// </summary>
            public IntPtr pOtherData;
            public long i64CreateTime;
            public ushort wFudgeTime;
            public ushort wOriginalXid;
            public ushort wError;
            public ushort wSigLength;
            public ushort wOtherLength;
            public byte cAlgNameLength;
            [MarshalAsAttribute(UnmanagedType.VariantBool)]
            public int bPacketPointers;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_WINS_DATA
        {
            public uint dwMappingFlag;
            public uint dwLookupTimeout;
            public uint dwCacheTimeout;
            public uint cWinsServerCount;
            /// <summary>
            /// uint Array of IP Addresses. Length: cWinsServerCount
            /// </summary>
            public IntPtr WinsServers;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_WINSR_DATA
        {
            public uint dwMappingFlag;
            public uint dwLookupTimeout;
            public uint dwCacheTimeout;
            /// <summary>
            /// String
            /// </summary>
            public IntPtr pNameResultDomain;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DNS_DHCID_DATA
        {
            public uint dwByteCount;
            /// <summary>
            /// Byte array with length dwByteCount
            /// </summary>
            public IntPtr DHCID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IP6_ADDRESS
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
            public uint[] IP6Dword;
        }
    }
}