using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System;

namespace QuickUnity
{
    public class SocketBase
    {
        public SocketBase(SocketBase.Type type)
        {
            socketType = type;
        }

        ~SocketBase()
        {
            Reset();
        }

        public void Disconnect()
        {
            DidDisconnect();
        }

        public virtual int Send(byte[] message)
        {
            // Check
            if (message == null || message.Length == 0) return 0;

            // Head
            int xHeadLength = IPAddress.NetworkToHostOrder(message.Length);
            byte[] head = BitConverter.GetBytes(xHeadLength);

            // Flush
            Msg msg = new Msg(++sentID, head, message);
            sendList.Add(msg);
            Flush();

            return msg.id;
        }

        public void Tick()
        {
            // Update tick time
            UpdateTickTime();

            OnTick();
        }

        protected virtual void OnTick() { }

        protected virtual void Reset()
        {
            if (sock != null) sock.Close();
            sock = null;
            sendList.Clear();
            UpdateTickTime();
        }

        protected void Flush()
        {
            if (sendList.Count == 0) return;
            for (int i = 0; i < sendList.Count; ++i)
            {
                Msg msg = sendList[i];
                sock.BeginSend(msg.bytes, 0, msg.bytes.Length, SocketFlags.None, new AsyncCallback(DidSent), msg);
            }
            finalSentTime = curTickTime;
            sendList.Clear();

        }

        protected void StartReceive()
        {
            ReallocReceivedCache(HeadLength);

            // Ready to receive
            sock.BeginReceive(
                receivedCache,
                0,
                HeadLength,
                SocketFlags.None,
                new AsyncCallback(DidReceiveHead),
                sock);
        }

        protected void UpdateTickTime()
        {
            curTickTime = (float)System.Environment.TickCount / 1000.0f;
        }

        protected void ReallocReceivedCache(int size)
        {
            if (receivedCache == null)
            {
                receivedCache = new byte[size];
                return;
            }
            if (receivedCache.Length >= size) return;

            byte[] newCache = new byte[size];
            Buffer.BlockCopy(receivedCache, 0, newCache, 0, receivedCache.Length);
            receivedCache = newCache;
        }

        protected bool CheckConnectState()
        {
            Socket s = sock;
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        }

        protected void DidDisconnect()
        {
            Reset();
            if (disconnectCallbacks != null) disconnectCallbacks(this);
        }

        protected void DidSent(System.IAsyncResult ar)
        {
            // Error check
            SocketError error;
            int sent = sock.EndSend(ar, out error);
            if (sent <= 0)
            {
                _lastError = error.ToString();
                return;
            }

            lastLog = "Message sent finished";
            Msg msg = (Msg)ar.AsyncState;
            if (sentCallbacks != null) sentCallbacks(this, msg.id);
        }

        protected void DidReceiveHead(System.IAsyncResult ar)
        {
            // Error check
            SocketError error;
            int read = sock.EndReceive(ar, out error);
            if (read <= 0)
            {
                _lastError = error.ToString();
                return;
            }


            // Get length
            var length = BitConverter.ToInt32(receivedCache, 0);
            int xLength = IPAddress.NetworkToHostOrder(length); ;

            // Check
            if (xLength >= maxMessageSize)
            {
                _lastError = string.Format("Receive a large message, size is {0}", xLength);
                Disconnect();
                return;
            }
            ReallocReceivedCache(HeadLength + xLength);

            // Receive body
            sock.BeginReceive(
                receivedCache,
                HeadLength,
                xLength,
                SocketFlags.None,
                new AsyncCallback(DidReceiveBody),
                xLength);
        }

        protected void DidReceiveBody(System.IAsyncResult ar)
        {
            // Error check
            SocketError error;
            int read = sock.EndReceive(ar, out error);
            if (read <= 0)
            {
                _lastError = error.ToString();
                return;
            }

            int length = Convert.ToInt32(ar.AsyncState);
            Msg msg = new Msg(++recvID, receivedCache, HeadLength, length);
            finalRecvTime = curTickTime;
            StartReceive();
            if (receiveCallbacks != null) receiveCallbacks(this, msg.bytes);
        }


        

        // Properties
        public const int HeadLength = sizeof(int);
        public SocketBase.Type socketType;
        public int maxMessageSize = QConfig.Network.maxMessageSize;
        public bool debug = false;

        // Events
        public event DidDisconnectCallback disconnectCallbacks;
        public event DidSentCallback sentCallbacks;
        public event DidReceiveCallback receiveCallbacks;

        // State and socket 
        protected Socket sock = null;

        // Time information
        protected float curTickTime = 0;
        protected float finalSentTime = 0;
        protected float finalRecvTime = 0;


        // Send
        protected List<Msg> sendList = new List<Msg>();

        // Receive 
        protected byte[] receivedCache = null;

        // Error and logs
        public string lastError { get { return _lastError; } }
        public string _lastError = null;
        public string lastLog { set { if (debug) Console.WriteLine(value); } }

        // Record
        protected int sentID = 0;
        protected int recvID = 0;

        // All defines
        public enum Type
        {
            Client = 0,
            Server = 1,
        }

        public struct Msg
        {
            public Msg(int id, byte[] bytes)
            {
                this.id = id;
                this.bytes = new byte[bytes.Length];
                Buffer.BlockCopy(bytes, 0, this.bytes, 0, bytes.Length);
            }

            public Msg(int id, byte[] bytes, int offset, int length)
            {
                this.id = id;
                this.bytes = new byte[length];
                Buffer.BlockCopy(bytes, offset, this.bytes, 0, length);
            }

            public Msg(int id, byte[] head, byte[] body)
            {
                this.id = id;
                this.bytes = new byte[head.Length + body.Length];
                Buffer.BlockCopy(head, 0, this.bytes, 0, head.Length);
                Buffer.BlockCopy(body, 0, this.bytes, head.Length, body.Length);
            }
            public byte[] bytes;
            public int id;
        }



        public delegate void DidDisconnectCallback(SocketBase socket);
        public delegate void DidReceiveCallback(SocketBase socket, byte[] bytes);
        public delegate void DidSentCallback(SocketBase socket, int sentID);
    }
}


