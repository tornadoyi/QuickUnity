using UnityEngine;
using System.Collections;
using QuickUnity;
using System;
using System.Net;

public class GameSocketListener :  ISocketListener
{
    public delegate void ConnectCallback(GameSocketListener listener);
    public delegate void DisconnectCallback(GameSocketListener listener);
    public delegate void ReceiveCallback(GameSocketListener listener, Message msg);

    public event ConnectCallback connectCallbacks;
    public event DisconnectCallback disconnectCallbacks;
    public event ReceiveCallback receiveCallbacks;

    private byte[] headerCache = new byte[sizeof(int)];

    public void Send(Message msg)
    {
        int xHeadLength = IPAddress.NetworkToHostOrder(msg.bytes.Length);
        byte[] head = BitConverter.GetBytes(xHeadLength);
        socket.Send(head);
        socket.Send(msg.bytes);
    }


    public override void OnConnect()
    {
        connectCallbacks.Invoke(this);
    }

    public override void OnDisconnect()
    {
        disconnectCallbacks.Invoke(this);
    }

    public override void OnTick()
    {
        // Seek header is existed
        if (socket.Seek(headerCache) < headerCache.Length) return;
        var msgLength = BitConverter.ToInt32(headerCache, 0);
        msgLength = IPAddress.NetworkToHostOrder(msgLength); ;

        // No message body
        if (headerCache.Length + msgLength < socket.receivedLength) return;

        // Read header and body
        var bytes = new byte[msgLength];
        socket.Receive(headerCache);
        socket.Receive(bytes);

        // Notify
        receiveCallbacks.Invoke(this, new Message(bytes));
    }

    
}
