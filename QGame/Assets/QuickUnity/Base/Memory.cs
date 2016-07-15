using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class Memory
    {
        public static string GetAddress(object o)
        {
            var h = System.Runtime.InteropServices.GCHandle.Alloc(o, System.Runtime.InteropServices.GCHandleType.Pinned);
            var addr = h.AddrOfPinnedObject();
            return "0x" + addr.ToString("X");
        }
    }
}

