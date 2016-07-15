using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    public class SocketManager : BaseManager<SocketManager>
    {
        protected override void OnDestroy()
        {
            socketDict.Clear();
            base.OnDestroy();
        }

        protected void Update()
        {
            Dictionary<int, SocketBase>.Enumerator e = socketDict.GetEnumerator();
            while(e.MoveNext())
            {
                e.Current.Value.Tick();
            }
        }

        public static int CreateClient(out SocketClient socket)
        {
            return instance._CreateClient(out socket);
        }

        public static void RemoveSocket(int id)
        {
            instance._RemoveSocket(id);
        }

        public static SocketBase GetSocket(int id)
        {
            return instance._GetSocket(id);
        }


        protected int _CreateClient(out SocketClient socket)
        {
            // Create socket
            socket = new SocketClient();

            // Save 
            int id = ++socketID;
            socketDict.Add(id, socket);
            return id;
        }

        public void _RemoveSocket(int id)
        {
            if (!socketDict.ContainsKey(id)) return;
            socketDict.Remove(id);
        }

        public SocketBase _GetSocket(int id)
        {
            if (!socketDict.ContainsKey(id)) return null;
            return socketDict[id];
        }

        // Containers
        protected Dictionary<int, SocketBase> socketDict = new Dictionary<int, SocketBase>();
        protected int socketID = 0;

    }
}


