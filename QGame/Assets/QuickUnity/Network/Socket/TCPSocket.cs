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

        ManualResetEvent connectEvent = new ManualResetEvent(false);
        ManualResetEvent sendEvent = new ManualResetEvent(false);

        public TCPSocket() : base(Protocol.TCP)
        {
            this.connectTimeout = QConfig.Network.tcpConnectTimeout;
        }

        protected override void OnDispose()
        {
            Disconnect();
            connectEvent.Close();
            sendEvent.Close();
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

            // Reset connect signal
            connectEvent.Reset();

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
            Disconnect(true);
        }

        protected void Disconnect(bool sendEvent)
        {
            if (!connected && !connecting) return;
            state = State.Disconnecting;

            // Stop threads
            this.connectEvent.Set();
            this.sendEvent.Set();

            // Close socket
            if(sock != null)
            {
                sock.Close();
                sock = null;
            }

            // Wait thread finished
            if (connectThread != null)
            {
                connectThread.Join();
                connectThread = null;
            }
            if (sendThread != null)
            {
                sendThread.Join();
                connectThread = null;
            }
            if (receiveThread != null)
            {
                receiveThread.Join();
                connectThread = null;
            }

            // Clear send buffer
            lock (sendBuffer)
            {
                sendBuffer.Clear();
            }

            state = State.Disconnected;
            if(sendEvent) NotifyDisconnectCallbacks();
        }

        protected override void OnTick()
        {
            switch(state)
            {
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
                this.sock.Blocking = true;

                var result = this.sock.BeginConnect(ipAddress, serverPort, new AsyncCallback((ar)=> 
                {
                    var sock = (Socket)ar.AsyncState;
                    sock.EndConnect(ar);
                    connectEvent.Set();

                }), this.sock);

                if(connectEvent.WaitOne( (int)(connectTimeout * 1000), false))
                {
                    // External exit command
                    if (state != State.Connecting)
                    {
                        // Nothing to do, ready to exit
                    }
                    else
                    {
                        // Check connected state
                        if (this.sock.Connected)
                        {
                            state = State.Connected;
                        }
                    }
                }
                else
                {
                    error = new Error(ErrorCode.SOCKET_CONNECT_TIMEOUT, "Connect failed, Timeout");
                }
            }
            catch(System.Exception e)
            {
                error = new Error(e);
            }
            finally
            {
                NotifyConnectCallbacks();
            }
            
            // Start work threads
            if(state == State.Connected)
            {
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
            else
            {
                // Ready to disconnect and cleanup
                SendEvent(delegate { Disconnect(false); });
            }

            // Exit
            Debug.Log("Connect thread exit");
        }

        protected void StartSendThread()
        {
            while (state == State.Connected)
            {
                try
                {
                    int length = 0;
                    int sendBufferLength = 0;
                    lock (sendBuffer)
                    {
                        if (sendBuffer.length > 0)
                        {
                            length = this.sock.Send(sendBuffer.bytes, 0, sendBuffer.length, SocketFlags.None);
                            if (length > 0)
                            {
                                sendBuffer.Read(length);
                                lastSendTime = DateTime.Now;
                            }
                        }
                        sendBufferLength = sendBuffer.length;
                    }

                    
                    if (sendBufferLength > 0) continue;
                    if ((DateTime.Now - lastSendTime).TotalSeconds < sendThreadSleepTime) continue;

                    sendEvent.Reset();
                    sendEvent.WaitOne();

                }
                catch(System.Exception e)
                {
                    error = e;
                    break;
                }
            }

            // Disconnect
            SendEvent(delegate { Disconnect(); });
            Debug.Log("Send thread exit");
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
                    length = 0;
                    if(sock.Poll(-1, SelectMode.SelectRead))
                    {
                        length = this.sock.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                        // Check network state
                        if(length == 0)
                        {
                            error = new Error(ErrorCode.SOCKET_DISCONNECT_DETECTED, "Detected socket has disconnected");
                            break;
                        }
                    }

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
                    // External close socket
                    if(e is ObjectDisposedException) break;
                    error = e;
                    break;
                }
            }

            // Disconnect
            SendEvent(delegate { Disconnect(); });
            Debug.Log("Receive thread exit");
        }

        protected void TriggerSendThread()
        {
            if (sendThread == null || !sendThread.IsAlive) return;

            bool awake = true;
            lock(sendBuffer)
            {
                awake = sendBuffer.length > 0;
            }

            if (awake)
                sendEvent.Set();
            else
                sendEvent.Reset();

        }
    }
}


