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
        public float sendThreadSleepTime { get; set; }

        protected Socket sock { get; set; }
        protected Thread connectThread { get; set; }
        protected Thread sendThread { get; set; }
        protected Thread receiveThread { get; set; }
        protected DateTime startConnectTime { get; set; }

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
            startConnectTime = DateTime.Now;

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

        public override void Disconnect()
        {
            if (!connected) return;
            state = State.Disconnecting;

            // Stop threads
            if(connectThread != null)
            {
                connectThread.Abort();
                connectThread = null;
            }
            if (sendThread != null)
            {
                sendThread.Abort();
                sendThread = null;
            }
            if (receiveThread != null)
            {
                receiveThread.Abort();
                receiveThread = null;
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
            switch(state)
            {
                case State.Connecting: CheckConnectTimeout(); break;
                case State.Connected: TriggerSendThread(); break;
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

            sendThread = new Thread(new ThreadStart(this.StartSendThread))
            {
                Name = "TCP Send",
                IsBackground = true
            };

            receiveThread = new Thread(new ThreadStart(this.StartReceiveThread))
            {
                Name = "TCP Receive",
                IsBackground = true
            };

            sendThread.Start();
            receiveThread.Start();
        }

        protected void StartSendThread()
        {
            while (state == State.Connected)
            {
                try
                {
                    int sendBufferLength = 0;
                    lock (sendBuffer)
                    {
                        if (sendBuffer.length > 0)
                        {
                            int length = sock.Send(sendBuffer.bytes, 0, sendBuffer.length, SocketFlags.None);
                            if (length > 0)
                            {
                                sendBuffer.Read(length);
                                lastSendTime = DateTime.Now;
                            }
                        }
                        sendBufferLength = sendBuffer.length;
                    }

                    if (sendBufferLength > 0) continue;
                    
                    try
                    {
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        continue;
                    }
                }
                catch(System.Exception e)
                {
                    error = e.ToString();
                    break;
                }
            }

            // Disconnect
            SendEvent(delegate { Disconnect(); });
        }

        protected void StartReceiveThread()
        {
            int length = 0;
            var buffer = new byte[1024];

            while (state == State.Connected)
            {
                try
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
                }
                catch (System.Exception e)
                {
                    error = e.ToString();
                    break;
                }
            }

            // Disconnect
            SendEvent(delegate { Disconnect(); });
        }


        protected void CheckConnectTimeout()
        {
            if (state != State.Connecting) return;
            var now = DateTime.Now;
            if ((now - startConnectTime).TotalSeconds > connectTimeout)
            {
                if (connectThread != null) connectThread.Abort();
                state = State.Disconnected;
                error = "Connect timeout";
                NotifyConnectCallbacks();
            }
        }

        protected void TriggerSendThread()
        {
            if (sendThread == null || !sendThread.IsAlive) return;

            bool awake = true;
            lock(sendBuffer)
            {
                awake = sendBuffer.length > 0;
            }

            if (!awake) return;
            if (sendThread.ThreadState == ThreadState.WaitSleepJoin ||
                sendThread.ThreadState == (ThreadState)36) // When sleep background = true thread, then state = 36
            {
                sendThread.Interrupt();
            }
        }
    }
}


