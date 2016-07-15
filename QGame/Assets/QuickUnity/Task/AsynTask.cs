using UnityEngine;
using System.Collections;
using System.Threading;


namespace QuickUnity
{
    public class AsyncTask : Task
    {
        #region Method

        protected override void OnStart()
        {
            TaskManager.CoroutineTask(new TaskManager.CoroutineTaskDelegate(this.WaitThreadDone));
            Toub.Threading.ManagedThreadPool.QueueUserWorkItem(new WaitCallback(this.AsynRun));
            return;
        }

        #endregion

        protected virtual void OnAsyncProcess() { throw new System.InvalidOperationException("Not Implement"); }

        protected override void Progress(float percent) { _asyncProgress = percent; }
        protected override void SetResultFailed(System.Exception e) { _asyncResult = false; _asyncError = e.ToString(); }
        protected override void SetResultFailed(string error, string suberror = default(string))
        {
            _asyncResult = false;
            _asyncError = error;
            if (string.IsNullOrEmpty(suberror)) return;
            _asyncError += "\n" + suberror;
        }
        protected override void SetResultSucess() { _asyncResult = true; }

        protected bool HasTimeout() { return _asyncHasTimeout; }

        #region Private Method
        IEnumerator WaitThreadDone()
        {
            while(!threadDone)
            {
                // Set progress
                base.Progress(_asyncProgress);

                // Sync timeout
                _asyncHasTimeout = hasTimeout;

                yield return null;
            }
            if (!_asyncResult) base.SetResultFailed(_asyncError);
            Done();
        }

        void AsynRun(System.Object state)
        {
            _threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            try
            {
                OnAsyncProcess();
            }
            catch(System.Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                threadDone = true;
            }                     
        }
        #endregion

        public int threadID { get { return _threadID; } }
        private volatile int _threadID = 0;

        public bool asyncResult { get { return _asyncResult; } }

        private volatile bool threadDone = false;
        private volatile bool _asyncResult = true;
        private volatile float _asyncProgress = 0.0f;
        private volatile string _asyncError = string.Empty;
        private volatile bool _asyncHasTimeout = false;
    }
}

