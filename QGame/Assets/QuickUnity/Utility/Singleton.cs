using UnityEngine;
using System.Collections;
using System;

namespace QuickUnity
{
    public class BaseSingleton : IDisposable
    {
        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose() { }
    }

    public class Singleton<T> : BaseSingleton where T : BaseSingleton, new()
    {
        protected Singleton()
        {
        }

        protected override void OnDispose()
        {
            _instance = null;
        }

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    //Debug.Log("Create " + typeof(T).Name);
                    _instance = new T();
                }
                return _instance;
            }
        }
        private static T _instance;
    }
}

