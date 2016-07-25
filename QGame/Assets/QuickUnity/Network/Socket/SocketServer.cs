using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System;

namespace QuickUnity
{
    public class SocketServer : SocketBase
    {
        public SocketServer()
            : base(SocketBase.Type.Server)
        {
            Reset();
        }

        public bool Listen()
        {
            if (state != State.Close) return true;

            // Parse IP and port
            IPEndPoint ipe = null;
            try
            {
                IPAddress ipa = null;
                if (!IPAddress.TryParse(ip, out ipa))
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                    ipa = hostEntry.AddressList[0];
                }
                ipe = new IPEndPoint(ipa, port);
            }
            catch (System.Exception ex)
            {
                _lastError = ex.ToString();
                return false;
            }
            if (ipe == null) return false;

            // Bind
            sock.Bind(ipe);

            // Listen
            sock.Listen(1000);
            state = State.Listen;

            // Begin accept
            

            return true;
        }

        public void Close()
        {

            connectedSocketDict.Clear();
            state = State.Close;
        }

        protected void BeginAccept()
        {
            if (state != State.Listen) return;
            sock.BeginAccept(new AsyncCallback((ar) =>
            {
                Socket sock_client = sock.EndAccept(ar);
                if (sock_client == null)
                {
                    Close();
                    return;
                }

                // Save
                connectedSocketDict.Add(sock_client, sock_client);

                // Receive


                // Continue accept
                BeginAccept();
            }), null);
        }

        protected override void OnTick()
        {
            // State dispatch
            switch (state)
            {
                case State.Close: break;
                case State.Listen: OnListen(); break;
            }
        }

        protected void OnListen()
        {
        }

        // Properties
        public string ip;
        public int port;
        public int maxConnected = 10000;

        // State
        protected State state = State.Close;

        // Connected socket dictionary
        protected Dictionary<Socket, Socket> connectedSocketDict = new Dictionary<Socket, Socket>();

        protected enum State
        {
            Close = 0,
            Listen,
        }
    }
}