using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using QuickUnity;
using System.Collections.Generic;

public class TestNetwork : MonoBehaviour {

    public Text state;
    public Text content;

    public Toggle batch;

    private int line;

    private ISocket socket;

    private int sendTime = 0;

    void Start()
    {
        var id = SocketManager.CreateTCPSocket();
        socket = SocketManager.FindSocket(id);

        var listener = new GameSocketListener();
        socket.SetListener(listener);
        socket.url = Setting.loginServerUrl;

        listener.connectCallbacks += OnConnect;
        listener.disconnectCallbacks += OnDisconnect;
        listener.receiveCallbacks += OnReceive;
        listener.batchReceiveCallbacks += OnBatchReceive;
    }


    public void Connect()
    {
        socket.Connect();
    }

    public void Disconnect()
    {
        socket.Disconnect();
    }

    public void Send()
    {
        if(batch.isOn)
        {
            for(int i=0; i<1000; ++i)
            {
                SendTo();
            }
        }
        else
        {
            SendTo();
        }
    }

    void SendTo()
    {
        ++sendTime;
        string str = string.Format("{0}{0}{0}{0}{0}", sendTime);
        (socket.listener as GameSocketListener).Send(new Message(str));
    }
	
    void Update()
    {
        state.text = socket.state.ToString();
    }

    public void OnConnect(GameSocketListener listener)
    {
        if(listener.connected)
        {
            AddInfo("connect success");
        }
        else
        {
            AddInfo("connect failed, " + socket.error);
        }
    }

    public void OnDisconnect(GameSocketListener listener)
    {
        AddInfo("disconnect");
    }

    public void OnReceive(GameSocketListener listener, Message msg)
    {
        AddInfo(string.Format("Receive: {0}", msg.content));
    }

    public void OnBatchReceive(GameSocketListener listener, List<Message> msg)
    {
        AddInfo(string.Format("Receive: {0} msg", msg.Count));
    }

    void AddInfo(string str)
    {
        ++line;
        if (line > 20)
        {
            content.text = string.Empty;
            line = 0;
        }
        content.text += str + "\n";
    }
}
