using UnityEngine;
using System.Collections;
using System.Threading;


namespace QuickUnity
{
    public class AsyncTask : Task 
    {
        public class ThreadTask
        {
            public volatile int threadID;
            public volatile string error = string.Empty;
            public volatile float progress;
            public volatile int result;
            public volatile int errorCode;
            public volatile bool hasTimeout;
            public bool success { get { return result == (int)Result.Success; } }
            public bool fail { get { return result == (int)Result.Fail; } }

            private Task master { get; set; }

            public void SetMasterTask(Task master)
            {
                if (master == null)
                {
                    Debug.LogError("Thread task need an master task");
                    return;
                }
                this.master = master;
            }

            public void AsynRun(System.Object state)
            {
                threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
                try
                {
                    OnAsyncProcess();
                }
                catch (System.Exception e)
                {
                    SetFail(e);
                }

            }

            protected void SetSuccess() { SetResult(Result.Success, ErrorCode.None, string.Empty); }
            protected void SetFail(string error = default(string)) { SetResult(Result.Fail, ErrorCode.Others, error); }
            protected void SetFail(System.Exception e) { SetResult(Result.Fail, ErrorCode.Others, e.ToString()); }
            protected void SetCancel(string error = default(string)) { SetResult(Result.Fail, ErrorCode.Cancel, error); }
            protected void SetTimeout(string error = default(string)) { SetResult(Result.Fail, ErrorCode.Timeout, error); }
            protected void SetProgress(float progress) { this.progress = progress; }
            private void SetResult(Result result, ErrorCode errorCode, string error)
            {
                if (result != Result.None) return;
                this.result = (int)result;
                this.errorCode = (int)errorCode;
                this.error = error;
            }

            protected virtual void OnAsyncProcess() { throw new System.InvalidOperationException("Not Implement"); }
        }
        

        private volatile ThreadTask threadTask;


        protected override void OnStart()
        {
            TaskManager.CoroutineTask(new TaskManager.CoroutineTaskDelegate(this.WaitThreadFinish));
            threadTask = CreateThreadTask();
            threadTask.SetMasterTask(this);
            OnSyncParameters(threadTask);
            Toub.Threading.ManagedThreadPool.QueueUserWorkItem(new WaitCallback(threadTask.AsynRun));
            return;
        }

        IEnumerator WaitThreadFinish()
        {
            while ((Result)threadTask.result == Result.None)
            {
                // Set progress
                base.SetProgress(threadTask.progress);

                // Sync timeout
                threadTask.hasTimeout = hasTimeout;

                yield return null;
            }

            // Sync
            OnSyncParameters(threadTask);
            var res = (Result)threadTask.result;
            var code = (ErrorCode)threadTask.errorCode;
            if (res == Result.Success)
            {
                SetSuccess();
            }
            else
            {
                switch(code)
                {
                    case ErrorCode.Cancel: SetCancel(threadTask.error); break;
                    case ErrorCode.Timeout: SetTimeout(threadTask.error); break;
                    default: SetFail(threadTask.error); break;
                }
            }

            // Finish,  Note: Must be the end of this function
            SetFinish();
        }

        protected virtual ThreadTask CreateThreadTask() { throw new System.InvalidOperationException("Not Implement"); }
        protected virtual void OnSyncParameters(ThreadTask task) { }
    }
}

