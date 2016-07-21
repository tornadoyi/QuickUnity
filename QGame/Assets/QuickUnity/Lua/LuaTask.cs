using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class LuaTask : CustomTask
    {
        public LuaTask()
        {
            this.finishAfterResult = false;
        }

        new public LuaTask Start(OnStartCallback callback)
        {
            base.Start(callback);
            return this;
        }
              
        new public void SetFinish()
        {
            base.SetFinish();
        }
    }
}

