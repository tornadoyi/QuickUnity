using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    public class QuickManager
    {
        public static void Start()
        {
            if (_isInit) return;
            InitManagers();
        }

        public static void Destory()
        {
            _isInit = false;
            GameObject.Destroy(qObject);
        }


        protected static void InitManagers()
        {
            qObject = new GameObject();
            qObject.name = "QuickUnity";
            GameObject.DontDestroyOnLoad(qObject);

            // Managers
            qObject.AddComponent<QuickBehaviour>();
            qObject.AddComponent<TaskManager>();
            qObject.AddComponent<AssetManager>();
            qObject.AddComponent<SocketManager>();
            qObject.AddComponent<LuaEngine>();
            qObject.AddComponent<HttpManager>();

            _isInit = true;
        }

        public static bool isInit { get { return _isInit; } }
        protected static bool _isInit = false;

        public static GameObject gameObject { get { return qObject; } }
        protected static GameObject qObject = null; 

    }


    
}

