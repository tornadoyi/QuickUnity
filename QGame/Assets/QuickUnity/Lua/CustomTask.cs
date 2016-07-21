using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class CustomTask : Task
    {
        public delegate void OnStartCallback(CustomTask task);

        public bool finishAfterResult { get; protected set;}
        protected OnStartCallback onStartCallback;

        public CustomTask(bool finishAfterResult = true)
        {
            this.finishAfterResult = finishAfterResult;
        }

        new public Task Start()
        {
            Debug.LogError("Task start fail, need specific function excute, please use Start(OnStartCallback callback) instead");
            return this;
        }

        public CustomTask Start(OnStartCallback callback)
        {
            if (callback == null)
            {
                Debug.LogError("Task start fail, function is invalid");
                return this;
            }
            onStartCallback = callback;
            base.Start();
            return this;
        }

        protected override void OnStart()
        {
            if (onStartCallback == null)
            {
                SetFail("Task start fail, No function can excute");
                return;
            }
            onStartCallback(this);
        }

        new public void SetSuccess()
        {
            base.SetSuccess();
            if (finishAfterResult)
            { 
                SetFinish();
            }
        }

        new public void SetFail(string error) 
        { 
            base.SetFail(error); 
            if (finishAfterResult)
            { 
                SetFinish(); 
            }
        }

        new public void SetCancel(string error = default(string)) 
        { 
            base.SetCancel(error); 
            if (finishAfterResult)
            { 
                SetFinish(); 
            }
        }
        new public void SetTimeout(string error = default(string)) 
        { 
            base.SetTimeout(error); 
            if (finishAfterResult)
            { 
                SetFinish(); 
            }
        }

        new protected void SetFinish()
        {
            if (state == State.Finish) return;
            base.SetFinish();
        }
    }
}

