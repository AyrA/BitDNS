using System.Net.Sockets;
using System;
using System.Text;
using System.Net;
using BitDNS;
using System.Text.RegularExpressions;
using System.IO;

namespace RPC
{
    public class Request
    {
        public bool lDNS
        { get; set; }
        public bool lNC
        { get; set; }
        public bool lAD
        { get; set; }

        private TcpClient c;
        private IPEndPoint EP;
        private byte[] buffer;
        private byte[] bufferOut;

        public Request(TcpClient c)
        {
            lDNS = lNC = lAD = true;
            EP = new IPEndPoint(IPAddress.Loopback, 8336);
            buffer = new byte[1024 * 1024];
            bufferOut = new byte[1024 * 1024];
            this.c = c;
            c.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(gotData), null);
        }

        private void gotData(IAsyncResult ar)
        {
            int i = 0;
            try
            {
                i = c.Client.EndReceive(ar);
            }
            catch
            {
                c = null;
                return;
            }
            if (i > 0)
            {
                string s=Encoding.UTF8.GetString(buffer, 0, i);
                //Test if it is just a connection check or a real lookup
                if (s.Contains("getinfo"))
                {
                    send(c, "{\"result\":{\"version\":123456},\"error\":null,\"id\":1}");
                }
                else if (s.Contains("name_show") && s.IndexOf(']')>s.IndexOf('['))
                {
                    //regex to check, if it is a DNS or Ad record
                    Regex REDNS = new Regex(@"dns/[a-z0-9\.]*");
                    Regex READ = new Regex(@"ad/[a-z0-9\.]*");
                    Match mDNS = REDNS.Match(s.ToLower());
                    Match mAD = READ.Match(s.ToLower());
                    if (mDNS.Captures.Count > 0 && lDNS)
                    {
                        //DNS
                        BMA[] Addr = Program.getTXT(mDNS.Captures[0].Value.Substring(mDNS.Captures[0].Value.IndexOf('/') + 1));
                        if (Addr != null && Addr.Length > 0)
                        {
                            send(c, "{\"result\":{\"name\":\"id/NAME\",\"value\":\"{\\\"name\\\":\\\"HUMAN NAME\\\",\\\"nick\\\":\\\"" + Addr[0].Label + "\\\",\\\"identity\\\":\\\"id/NAME\\\",\\\"bitmessage\\\":\\\"" + Addr[0].Address + "\\\"}\"},\"error\":null,\"id\":1}");
                        }
                        else
                        {
                            send(c, "{\"result\":{\"name\":\"id/NAME\",\"value\":\"{\\\"name\\\":\\\"HUMAN NAME\\\",\\\"nick\\\":\\\"NICKNAME\\\",\\\"identity\\\":\\\"id/NAME\\\"}\"},\"error\":null,\"id\":1}");
                        }
                    }
                    else if (mAD.Captures.Count > 0 && lAD)
                    {
                        //AD
                        string Addr = AddressBook.Get(mAD.Captures[0].Value.Substring(mAD.Captures[0].Value.IndexOf('/') + 1));
                        if (Addr != null && Addr.Length > 0)
                        {
                            send(c, "{\"result\":{\"name\":\"id/NAME\",\"value\":\"{\\\"name\\\":\\\"HUMAN NAME\\\",\\\"nick\\\":\\\"null\\\",\\\"identity\\\":\\\"id/NAME\\\",\\\"bitmessage\\\":\\\"" + Addr + "\\\"}\"},\"error\":null,\"id\":1}");
                        }
                        else
                        {
                            send(c, "{\"result\":{\"name\":\"id/NAME\",\"value\":\"{\\\"name\\\":\\\"HUMAN NAME\\\",\\\"nick\\\":\\\"NICKNAME\\\",\\\"identity\\\":\\\"id/NAME\\\"}\"},\"error\":null,\"id\":1}");
                        }
                    }
                    else if (lNC)
                    {
                        //NC
                        try
                        {
                            TcpClient NC = new TcpClient();
                            NC.Connect(EP);
                            NetworkStream NS = new NetworkStream(NC.Client, false);
                            StreamWriter SW = new StreamWriter(NS);
                            StreamReader SR = new StreamReader(NS);

                            SW.Write(s);
                            SW.Flush();
                            string answer = SR.ReadToEnd();
                            SW.Close();
                            SW.Dispose();
                            SR.Close();
                            SR.Dispose();
                            NS.Close();
                            NS.Dispose();
                            NC.Client.Disconnect(false);

                            send(c, answer);
                        }
                        catch
                        {
                            send(c, "{\"result\":null,\"error\":\"Cannot connect to namecoind at " + EP.ToString() + "\",\"id\":1}");
                        }
                    }
                    else
                    {
                        send(c, "{\"result\":null,\"error\":\"No results for your query\",\"id\":1}");
                    }
                }
            }
        }

        /// <summary>
        /// Sends json text data via HTTP to the socket. Adding, the header if not present
        /// </summary>
        /// <param name="cli"></param>
        /// <param name="s"></param>
        public void send(TcpClient cli, string s)
        {
            if (cli != null)
            {
                if (!s.StartsWith("HTTP/1.1"))
                {
                    s = string.Format(@"HTTP/1.1 200 OK
Content-Length: {0}
Connection: Close
Content-Type: application/json

{1}", s.Length, s);
                }
                Console.WriteLine(s);
                byte[] b = Encoding.UTF8.GetBytes(s);
                try
                {
                    cli.Client.Send(b, 0, b.Length, SocketFlags.None);
                    cli.Client.Disconnect(false);
                }
                catch
                {
                    cli = null;
                }
            }
            else
            {
                Console.WriteLine("ERR: Connection closed");
            }
        }

        public void send(TcpClient cli, byte[] data, int from, int length)
        {
            if (cli != null)
            {
                byte[] temp = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    if (data[from + i] > 31)
                    {
                        temp[i] = data[from + i];
                    }
                    else
                    {
                        temp[i] = 0x2E;
                    }
                }
                try
                {
                    cli.Client.Send(data, from, length, SocketFlags.None);
                }
                catch
                {
                    cli = null;
                }
            }
            else
            {
                Console.WriteLine("ERR: Connection closed");
            }
        }

    }
}
