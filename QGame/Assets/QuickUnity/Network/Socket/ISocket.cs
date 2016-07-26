using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System;

namespace QuickUnity
{
    public abstract class ISocket : IDisposable
    {
        protected delegate void EventDelegate();

        public enum State
        {
            Disconnected,
            Connecting,
            Connected,
            Disconnecting
        }

        public enum Protocol
        {
            TCP = 0,
            UDP,
        }

        public ISocketListener listener { get; protected set; }

        public Protocol protocol { get; protected set; }

        public State state { get; protected set; }
        public bool disconnected { get { return state == State.Disconnected; } }
        public bool connecting { get { return state == State.Connecting; } }
        public bool connected { get { return state == State.Connected; } }
        public bool disconnecting { get { return state == State.Disconnecting; } }

        
        public string url { get; protected set; }
        public string serverAddress { get; protected set; }
        public int serverPort { get; protected set; }
        public string urlProtocol { get; protected set; }
        public string urlPath { get; protected set; }


        public float sendTimeout { get; set; }
        public float receiveTimeout { get; set; }


        public string error
        {
            get { return _error; }
            set
            {
                lock(_error)
                {
                    _error = value;
                    Logger.LogError(_error);
                }
            }
        }
        private string _error = string.Empty;

        // Events
        private List<EventDelegate> eventList = new List<EventDelegate>();


        // Buffers
        protected AlignBuffer sendBuffer { get; set; }
        protected RecycleBuffer receiveBuffer { get; set; }


        public ISocket(Protocol protocol)
        {
            this.protocol = protocol;

            this.sendTimeout = QConfig.Network.socketSendTimeout;
            this.receiveTimeout = QConfig.Network.socketReceiveTimeout;
            this.sendBuffer = new AlignBuffer();
            this.receiveBuffer = new RecycleBuffer();

            this.sendBuffer.maxCapacity = QConfig.Network.socketBufferLength;
            this.receiveBuffer.maxCapacity = QConfig.Network.socketBufferLength;

            SetListener(new DefaultSocketListener());
        }

        public void Dispose()
        {
            OnDispose();
        }


        public void SetListener(ISocketListener listener)
        {
            if (listener == null) return;
            listener.BindSocket(this);
            this.listener = listener;
        }

        public virtual bool Connect()
        {
            if (!disconnected)
            {
                error = string.Format("Connect failed, Current state is {0}", state);
                return false;
            }
            if (string.IsNullOrEmpty(this.serverAddress))
            {
                string serverAddress = string.Empty;
                ushort serverPort = 0;
                string urlProtocol = string.Empty;
                string urlPath = string.Empty;
                if(!TryParseURL(url, out serverAddress, out serverPort, out urlProtocol, out urlPath))
                {
                    _error = string.Format("Connect failed, Parse url:{0} failed", url);
                    return false;
                }
                this.serverAddress = serverAddress;
                this.serverPort = serverPort;
                this.urlProtocol = urlProtocol;
                this.urlPath = urlPath;
            }

            return true;
        }

        public virtual void Send(byte[] bytes) { Send(bytes, 0, bytes.Length); }

        public virtual void Send(byte[] bytes, int length) { Send(bytes, 0, length); }

        public virtual void Send(byte[] bytes, int offset, int length)
        {
            lock(sendBuffer)
            {
                if (sendBuffer.availableCapacity < length)
                {
                    throw new ArgumentException(string.Format("Not enough available capacity({0})", sendBuffer.availableCapacity));
                }
                sendBuffer.Write(bytes, offset, length);
            }
        }

        public virtual int Receive(byte[] bytes) { return Receive(bytes, 0, bytes.Length); }

        public virtual int Receive(byte[] bytes, int length) { lock (receiveBuffer) { return Receive(bytes, 0, length); } }

        public virtual int Receive(byte[] bytes, int offset, int length) { lock (receiveBuffer) { return receiveBuffer.Read(bytes, offset, length); } }

        public virtual int Seek(byte[] bytes) { return Seek(bytes, 0, bytes.Length); }

        public virtual int Seek(byte[] bytes, int length) { return Seek(bytes, 0, length); }

        public virtual int Seek(byte[] bytes, int offset, int length) { lock (receiveBuffer) { return receiveBuffer.Seek(bytes, offset, length); } }

        public void Tick()
        {
            OnTick();
            DispatchEvents();
            listener.OnTick();
        }


        protected bool TryParseURL(string url, out string address, out ushort port, out string urlProtocol, out string urlPath)
        {
            address = string.Empty;
            port = 0;
            urlProtocol = string.Empty;
            urlPath = string.Empty;
            string text = url;
            bool flag = string.IsNullOrEmpty(text);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                int num = text.IndexOf("://");
                bool flag2 = num >= 0;
                if (flag2)
                {
                    urlProtocol = text.Substring(0, num);
                    text = text.Substring(num + 3);
                }
                num = text.IndexOf("/");
                bool flag3 = num >= 0;
                if (flag3)
                {
                    urlPath = text.Substring(num);
                    text = text.Substring(0, num);
                }
                num = text.LastIndexOf(':');
                bool flag4 = num < 0;
                if (flag4)
                {
                    result = false;
                }
                else
                {
                    bool flag5 = text.IndexOf(':') != num && (!text.Contains("[") || !text.Contains("]"));
                    if (flag5)
                    {
                        result = false;
                    }
                    else
                    {
                        address = text.Substring(0, num);
                        string s = text.Substring(num + 1);
                        bool flag6 = ushort.TryParse(s, out port);
                        result = flag6;
                    }
                }
            }
            return result;
        }

        protected internal static IPAddress GetIPAddress(string serverIp)
        {
            IPAddress iPAddress = null;
            bool flag = IPAddress.TryParse(serverIp, out iPAddress);
            IPAddress result;
            if (flag)
            {
                result = iPAddress;
            }
            else
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(serverIp);
                IPAddress[] addressList = hostEntry.AddressList;
                IPAddress[] array = addressList;
                for (int i = 0; i < array.Length; i++)
                {
                    IPAddress iPAddress2 = array[i];
                    bool flag2 = iPAddress2.AddressFamily == AddressFamily.InterNetworkV6;
                    if (flag2)
                    {
                        result = iPAddress2;
                        return result;
                    }
                }
                IPAddress[] array2 = addressList;
                for (int j = 0; j < array2.Length; j++)
                {
                    IPAddress iPAddress3 = array2[j];
                    bool flag3 = iPAddress3.AddressFamily == AddressFamily.InterNetwork;
                    if (flag3)
                    {
                        result = iPAddress3;
                        return result;
                    }
                }
                result = null;
            }
            return result;
        }



        protected void DispatchEvents()
        {
            lock (eventList)
            {
                for(int i=0; i< eventList.Count; ++i)
                {
                    eventList[i].Invoke();
                }
                eventList.Clear();
            }
        }

        protected void NotifyConnectCallbacks() { SendEvent(delegate { listener.OnConnect(); }); }

        protected void NotifyDisconnectCallbacks() { SendEvent(delegate { listener.OnDisconnect(); }); }


        protected void SendEvent(EventDelegate del)
        {
            lock (eventList)
            {
                this.eventList.Add(del);
            }
        }

        protected virtual void OnTick() { }

        protected virtual void OnDispose() { }
    }
}


