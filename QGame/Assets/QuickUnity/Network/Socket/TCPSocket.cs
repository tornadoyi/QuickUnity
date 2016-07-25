using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Threading;

namespace QuickUnity
{
    public class TCPSocket : ISocket, IDisposable
    {
        protected Socket sock { get; set; }

        public TCPSocket() : base(Protocol.TCP)
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public override bool Connect()
        {
            if (!base.Connect()) return false;

            state = State.Connecting;
            new Thread(new ThreadStart(this.StartConnectThread))
            {
                Name = "TCP Connect",
                IsBackground = true
            }.Start();

            return true;
        }

        protected void StartConnectThread()
        {
            try
            {
                var ipAddress = GetIPAddress(serverAddress);
                if (ipAddress == null)
                {
                    throw new ArgumentException(string.Format("Connect failed, Get ip address:{0} failed", serverAddress));
                }
                this.sock = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.sock.NoDelay = true;
                this.sock.SendTimeout = (int)(sendTimeout * 1000.0f);
                this.sock.ReceiveTimeout = (int)(receiveTimeout * 1000.0f);
                this.sock.Connect(ipAddress, serverPort);
                state = State.Connected;
            }
            catch(System.Exception e)
            {
                error = e.ToString();
                state = State.Disconnecting;
                NotifyConnectCallbacks();
                return;
            }

            new Thread(new ThreadStart(this.ReceiveLoopThread))
            {
                Name = "TCP Receive",
                IsBackground = true
            }.Start();
        }


        protected void ReceiveLoopThread()
        {

        }

    }
}


