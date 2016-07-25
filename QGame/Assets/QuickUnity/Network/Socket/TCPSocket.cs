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

            new Thread(new ThreadStart(this.WorkThread))
            {
                Name = "TCP Receive",
                IsBackground = true
            }.Start();
        }


        protected void WorkThread()
        {
            int length = 0;
            var buffer = new byte[1024];

            try
            {
                while (state == State.Connected)
                {
                    // Receive 
                    length = sock.Receive(buffer, buffer.Length, SocketFlags.None);

                    // Write to receive buffer
                    if (length > 0)
                    {
                        lock (receiveBuffer)
                        {
                            receiveBuffer.Write(buffer, length);
                        }

                        // Update buffer size
                        if (length >= buffer.Length)
                        {
                            buffer = new byte[(int)(buffer.Length * 1.5)];
                        }

                    }

                    // Send
                    lock (sendBuffer)
                    {
                        if (sendBuffer.length > 0)
                        {
                            length = sock.Send(sendBuffer.bytes, 0, sendBuffer.length, SocketFlags.None);
                            if (length > 0)
                            {
                                sendBuffer.Remove(length);
                            }
                        }
                    }
                }
            }
            catch(System.Exception e)
            {
                error = e.ToString();
            }


        }





    }
}


