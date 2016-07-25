using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System;

namespace QuickUnity
{
    public class SocketClient : SocketBase
    {
        public SocketClient()
            : base(SocketBase.Type.Client)
        {
            Reset();
        }

        ~SocketClient()
        {
            Reset();
        }


        public bool Connect()
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


            // Create new socket
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Blocking = false;

            // Connecting
            sock.BeginConnect(ipe, new System.AsyncCallback(DidConnect), sock);
            state = State.Connecting;
            startConnectTime = curTickTime;

            return true;
        }


        protected override void OnTick()
        {
            // State dispatch
            switch (state)
            {
                case State.Close: break;
                case State.Connecting: OnConnecting(); break;
                case State.Connected: OnConnected(); break;
            }
        }

        protected void OnConnecting()
        {
            if (curTickTime - startConnectTime > connectTimeout)
            {
                _lastError = "Connect timeout";
                Disconnect();
                state = State.Close;
            }
        }


        protected void OnConnected()
        {
            // Check socket state
            if (!CheckConnectState())
            {
                Disconnect();
                state = State.Close;
                return;
            }
        }

        // Events
        protected void DidConnect(System.IAsyncResult ar)
        {
            if (!sock.Connected)
            {
                Disconnect();
                return;
            }
            state = State.Connected;
            Flush();
            StartReceive();
            if (connectCallbacks != null) connectCallbacks(this);
        }


        // Properties
        public string ip;
        public int port;
        public float connectTimeout = float.MaxValue;

        //Events
        public event DidConnectCallback connectCallbacks;

        // Time information
        protected float startConnectTime = 0;

        // State
        protected State state = State.Close;

        public delegate void DidConnectCallback(SocketBase socket);

        protected enum State
        {
            Close = 0,
            Connecting = 1,
            Connected = 2,
        }
    }
}

