using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    public class SocketManager : BaseManager<SocketManager>
    {
        private Dictionary<int, ISocket> sockets = new Dictionary<int, ISocket>();

        private int serialID { get; set; }


        public static int CreateTCPSocket() { return instance.Create<TCPSocket>(); }

        public static void RemoveSocket(int id) { instance.Remove(id); }

        public static ISocket Findocket(int id) { return instance.Find(id); }


        public int Create<T>() where T : ISocket, new()
        {
            var socket = new T();
            var id = GenerateID();
            sockets.Add(id, socket);
            return id;
        }
        
        public void Remove(int id)
        {
            var socket = Find(id);
            if (socket == null) return;
            socket.Dispose();
            sockets.Remove(id);
        }

        public ISocket Find(int id)
        {
            ISocket socket = null;
            sockets.TryGetValue(id, out socket);
            return socket;
        }


        private int GenerateID()
        {
            serialID += 1;
            return serialID;
        }
    }
}


