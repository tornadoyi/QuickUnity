using UnityEngine;
using System.Collections;
using QuickUnity;
using System;
using System.Net;
using System.Collections.Generic;

public class GameSocketListener :  ISocketListener
{
    public delegate void ConnectCallback(GameSocketListener listener);
    public delegate void DisconnectCallback(GameSocketListener listener);
    public delegate void ReceiveCallback(GameSocketListener listener, Message msg);
    public delegate void BatchReceiveCallback(GameSocketListener listener, List<Message> msg);

    public event ConnectCallback connectCallbacks;
    public event DisconnectCallback disconnectCallbacks;
    public event ReceiveCallback receiveCallbacks;
    public event BatchReceiveCallback batchReceiveCallbacks;

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
        var msg = TryReceive();
        if (msg == null) return;
        // Notify

        var list = new List<Message>();
        while (msg != null)
        {
            list.Add(msg);
            msg = TryReceive();
        }
        if (list.Count == 1)
            receiveCallbacks.Invoke(this, list[0]);
        else
            batchReceiveCallbacks.Invoke(this, list);
    }

    Message TryReceive()
    {
        // Seek header is existed
        if (socket.Seek(headerCache) < headerCache.Length) return null;
        var msgLength = BitConverter.ToInt32(headerCache, 0);
        msgLength = IPAddress.NetworkToHostOrder(msgLength); ;

        // No message body
        if (headerCache.Length + msgLength > socket.receivedLength) return null;

        // Read header and body
        var bytes = new byte[msgLength];
        socket.Receive(headerCache);
        socket.Receive(bytes);

        return new Message(bytes);
        
    }
}
