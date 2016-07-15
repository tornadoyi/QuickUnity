using UnityEngine;
using System.Collections;
using LuaInterface;
using SLua;
using System;

namespace QuickUnity
{
    public class SafeLuaCoroutine
    {
        static public Coroutine StartCoroutine(MonoBehaviour mb, object e, CoroutineCallback d)
        {
            if(mb == null || d == null)
            {
                Debug.LogError("Invalid arguments");
                return null;
            }

            return mb.StartCoroutine(Excute(e, d));
        }

        static public Coroutine NextFrame(MonoBehaviour mb, CoroutineCallback d)
        {
            return mb.StartCoroutine(_NextFrame(d));
        }

        static private IEnumerator Excute(object y, CoroutineCallback act)
        {
            if (y is IEnumerator)
            {
                IEnumerator e = (IEnumerator)y;
                yield return e;
//                 if(e !=null && e.MoveNext())
//                 {
//                     yield return e.Current;
//                 }
            }   
            else if(y is YieldInstruction)
            {
                yield return y;
            }
            else
            {
                Debug.LogError("Invalid yield object");
            }

            if (act != null) act();
        }

        static private IEnumerator _NextFrame(CoroutineCallback act)
        {
            yield return null;
            if (act != null) act();
        }

        public delegate void CoroutineCallback();
    }
}


