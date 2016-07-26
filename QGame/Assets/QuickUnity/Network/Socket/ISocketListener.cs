using UnityEngine;
using System.Collections;
using System;

namespace QuickUnity
{
    public abstract class ISocketListener 
    {
        public bool disconnected { get { return state == ISocket.State.Disconnected; } }
        public bool connecting { get { return state == ISocket.State.Connecting; } }
        public bool connected { get { return state == ISocket.State.Connected; } }
        public bool disconnecting { get { return state == ISocket.State.Disconnecting; } }

        public ISocket.State state { get { return socket.state; } }
        protected ISocket socket { get; set; }

        public void BindSocket(ISocket socket)
        {
            if (socket == null) return;
            this.socket = socket;
        }

        public abstract void OnConnect();

        public abstract void OnDisconnect();

        public abstract void OnTick();
    }

    public class DefaultSocketListener : ISocketListener
    {
        public override void OnConnect() { }

        public override void OnDisconnect() { }

        public override void OnTick() { }
    }
}


