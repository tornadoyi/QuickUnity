using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Threading;

namespace QuickUnity
{
    public class TCPSocket : ISocket
    {
        public float connectTimeout { get; set; }

        protected Socket sock { get; set; }

        protected Thread connectThread { get; set; }

        protected Thread workThread { get; set; }

        protected long startConnectTime { get; set; }

        public TCPSocket() : base(Protocol.TCP)
        {
            this.connectTimeout = QConfig.Network.tcpConnectTimeout;
        }

        protected override void OnDispose()
        {
            Disconnect();
        }

        public override bool Connect()
        {
            if (!base.Connect()) return false;
            state = State.Connecting;
            startConnectTime = DateTime.Now.ToUniversalTime().Ticks;

            // Clear receive buffer
            lock (receiveBuffer)
            {
                receiveBuffer.Clear();
            }

            // Launch connect thread
            connectThread = new Thread(new ThreadStart(this.StartConnectThread))
            {
                Name = "TCP Connect",
                IsBackground = true
            };
            connectThread.Start();

            return true;
        }

        public virtual void Disconnect()
        {
            if (!connected) return;
            state = State.Disconnecting;

            // Stop threads
            if(connectThread != null)
            {
                connectThread.Abort();
                connectThread = null;
            }
            if(workThread != null)
            {
                workThread.Abort();
                workThread = null;
            }

            // Close socket
            sock.Close();
            sock = null;
            

            // Clear send buffer
            lock(sendBuffer)
            {
                sendBuffer.Clear();
            }

            state = State.Disconnected;
            NotifyDisconnectCallbacks();
        }

        protected override void OnTick()
        {
            // Check connect timeout
            if(state == State.Connecting)
            {
                long now = DateTime.Now.ToUniversalTime().Ticks;
                if((float)(now - startConnectTime) > connectTimeout)
                {
                    if (connectThread != null) connectThread.Abort();
                    state = State.Disconnected;
                }
            }
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
                NotifyConnectCallbacks();
            }
            catch(System.Exception e)
            {
                error = e.ToString();
                state = State.Disconnected;
                NotifyConnectCallbacks();
                return;
            }

            workThread = new Thread(new ThreadStart(this.WorkThread))
            {
                Name = "TCP Receive",
                IsBackground = true
            };
            workThread.Start();
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
                                sendBuffer.Read(length);
                            }
                        }
                    }
                }
            }
            catch(System.Exception e)
            {
                error = e.ToString();
            }

            // Disconnect
            SendEvent(delegate { Disconnect(); });
        }





    }
}


