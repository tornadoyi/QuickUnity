using UnityEngine;
using System.Collections;
using System;

namespace QuickUnity
{
    public class SocketMessage : MonoBehaviour
    {
        public byte[] bytes { get; private set; }
        public int id { get; private set; }

        public SocketMessage(int id, byte[] bytes)
        {
            this.id = id;
            this.bytes = new byte[bytes.Length];
            Buffer.BlockCopy(bytes, 0, this.bytes, 0, bytes.Length);
        }
    }
}


