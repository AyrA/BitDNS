using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace RPC
{
    public class XMLsrv
    {
        private TcpListener s;
        private List<Request> Connections;
        public XMLsrv(int LocalPort)
        {
            Connections = new List<Request>();
            s = new TcpListener(IPAddress.Loopback, LocalPort);
            s.Start();
            s.BeginAcceptTcpClient(new AsyncCallback(gotConnection), null);
        }

        private void gotConnection(IAsyncResult ar)
        {
            try
            {
                var sc = new Request(s.EndAcceptTcpClient(ar));

                Connections.Add(sc);
                s.BeginAcceptTcpClient(new AsyncCallback(gotConnection), null);

            }
            catch
            {

            }
        }
    }
}
