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

        public static ISocket FindSocket(int id) { return instance.Find(id); }


        protected override void OnDestroy()
        {
            var e = sockets.GetEnumerator();
            while(e.MoveNext())
            {
                e.Current.Value.Dispose();
            }
            sockets.Clear();
            base.OnDestroy();
        }

        void Update()
        {
            var e = sockets.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Value.Tick();
            }
        }

        private int Create<T>() where T : ISocket, new()
        {
            var socket = new T();
            var id = GenerateID();
            sockets.Add(id, socket);
            return id;
        }

        private void Remove(int id)
        {
            var socket = Find(id);
            if (socket == null) return;
            socket.Dispose();
            sockets.Remove(id);
        }

        private ISocket Find(int id)
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


