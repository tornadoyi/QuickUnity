using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    /// <summary>
    /// Task System execute task in Coroutine 
    /// </summary>
    
    public class TaskManager : BaseManager<TaskManager>
    {
        public delegate IEnumerator CoroutineTaskDelegate();

        public static Coroutine CoroutineTask(CoroutineTaskDelegate d)
        {
            if (d == null)
                return null;

            return instance.StartCoroutine(d());
        }

        public static void StopCoroutineTask(Coroutine co)
        {
            if(co == null) { Debug.LogError("Invalid coroutine"); return; }
            instance.StopCoroutine(co);
        }

    }



    /// <summary>
    /// Task means a job which will be executed in Coroutine Or Thread or Immediately, depend on implementation . 
    /// This is just a interface.
    /// </summary>

    public class Task
    {
        public delegate void FinishCallback(Task task);
        public delegate void ProgressCallback(Task task, float percent);

        class Callback
        {
            public Callback(FinishCallback callback, Result result, ErrorCode errorCode)
            {
                this.callback = callback;
                this.result = result;
                this.errorCode = errorCode;
            }
            public FinishCallback callback;
            public Result result;
            public ErrorCode errorCode;
        }

        protected enum State { Sleep = 0, Running, Finish}
        protected enum Result { None = 0, Success = 1, Fail = 2, Any = Success | Fail}
        protected enum ErrorCode { None = 0, Cancel = 1, Timeout = 2, Others = 4, Any = Cancel | Timeout | Others}

        protected State state { get; set; }
        public bool sleep { get { return state == State.Sleep; } }
        public bool running { get { return state == State.Running; } }
        public bool finish { get { return state == State.Finish; } }

        protected Result result { get; set; }
        public bool success { get { return result == Result.Success; } }
        public bool fail { get { return result == Result.Fail; } }

        public float progress { get; private set; }

        protected ErrorCode errorCode { get; set; }
        public string error { get; protected set; }

        public bool enableRetry { get; private set;}
        public int retryCount { get; set; }
        public int curRetryCount { get; private set; }

        public float startTime { get; private set; }
        public float endTime { get; private set; }
        public float timeout
        {
            get { return _timeout; }
            set
            {
                _timeout = value;
                if (_timeout >= 0 && co_timeout != null) { co_timeout = TaskManager.CoroutineTask(new TaskManager.CoroutineTaskDelegate(WaitForTimeout)); }
            }
        }
        protected float _timeout = -1;
        private Coroutine co_timeout;
        public float costTime { get { return startTime == 0 ? 0 : endTime == 0 ? Time.time - startTime : endTime - startTime; } }
        public bool hasTimeout { get { return timeout < 0 ? false : (costTime - startTime >= _timeout); } }

        private List<Callback> finishCallbacks;
        private List<ProgressCallback> progressCallbacks;

        public Task()
        {
#if UNITY_EDITOR
            TaskMonitor.RecordTaskCreate(this);
#endif
        }

        public Task Start()
        {
            if (state != State.Sleep) { Debug.LogError("Task has been used, cannot start again"); return this; }
            state = State.Running;
            result = Result.None;
            progress = 0;
            errorCode = ErrorCode.None;
            error = string.Empty;
            startTime = Time.time;
            endTime = 0;

            ++curRetryCount;
            OnStart();
#if UNITY_EDITOR
            if(curRetryCount == 1) TaskMonitor.RecordTaskStart(this);
#endif
            return this;
        }


        public Task Finish(FinishCallback callback)
        {
            if (callback == null) return this;
            if(state == State.Finish)
            {
                callback(this);
            }
            else
            {
                if (finishCallbacks == null) finishCallbacks = new List<Callback>();
                finishCallbacks.Add(new Callback(callback, Result.Any, ErrorCode.Any));
            }
            return this;
        }

        public Task Success(FinishCallback callback)
        {
            if (callback == null) return this;
            if (state == State.Finish)
            {
                if(result == Result.Success) callback(this);
            }
            else
            {
                if (finishCallbacks == null) finishCallbacks = new List<Callback>();
                finishCallbacks.Add(new Callback(callback, Result.Success, ErrorCode.None));
            }
            return this;
        }

        public Task Fail(FinishCallback callback)
        {
            if (callback == null) return this;
            if (state == State.Finish)
            {
                if (result == Result.Fail) callback(this);
            }
            else
            {
                if (finishCallbacks == null) finishCallbacks = new List<Callback>();
                finishCallbacks.Add(new Callback(callback, Result.Fail, ErrorCode.Any));
            }
            return this;
        }

        public Task Cancel(FinishCallback callback)
        {
            if (callback == null) return this;
            if (state == State.Finish)
            {
                if (result == Result.Fail && errorCode == ErrorCode.Cancel) callback(this);
            }
            else
            {
                if (finishCallbacks == null) finishCallbacks = new List<Callback>();
                finishCallbacks.Add(new Callback(callback, Result.Fail, ErrorCode.Cancel));
            }
            return this;
        }

        public Task Timeout(FinishCallback callback)
        {
            if (callback == null) return this;
            if (state == State.Finish)
            {
                if (result == Result.Fail && errorCode == ErrorCode.Timeout) callback(this);
            }
            else
            {
                if (finishCallbacks == null) finishCallbacks = new List<Callback>();
                finishCallbacks.Add(new Callback(callback, Result.Fail, ErrorCode.Timeout));
            }
            return this;
        }

        public Task Progress(ProgressCallback callback)
        {
            if (callback == null) return this;
            if (state == State.Finish)
            {
                callback(this, progress);
            }
            else
            {
                if (progressCallbacks == null) progressCallbacks = new List<ProgressCallback>();
                progressCallbacks.Add(callback);
            }
            return this;
        }

        public IEnumerator WaitForFinish()
        {
            while (!finish)
            {
                yield return null;
            }
        }

        protected IEnumerator WaitForTimeout()
        {
            while(state != State.Finish)
            {
                if (!hasTimeout) yield return null;
                SetTimeout();
            }
            co_timeout = null;
        }

        protected void SetSuccess() { SetResult(Result.Success, ErrorCode.None, string.Empty); }
        protected void SetFail(string error) { SetResult(Result.Fail, ErrorCode.Others, error); }
        protected void SetFail(System.Exception e) { SetResult(Result.Fail, ErrorCode.Others, e.ToString()); }
        protected void SetFail(string error, string suberror) { SetResult(Result.Fail, ErrorCode.Others, error + "\n" + suberror); }
        protected void SetCancel(string error = default(string)) { SetResult(Result.Fail, ErrorCode.Cancel, error); }
        protected void SetTimeout(string error = default(string)) { SetResult(Result.Fail, ErrorCode.Timeout, error); }
        protected void SetProgress(float progress)
        {
            this.progress = progress;
            NotifyProgressCallbacks();
        }

        // Only call at child class
        protected void SetFinish()
        {
            if (state == State.Finish) return;
            state = State.Finish;
            if (result == Result.None) result = Result.Success;
            endTime = Time.time;

            if(enableRetry && curRetryCount < retryCount)
            {
                state = State.Sleep;
                Start();
                return;
            }

            NotifyFinishCallbacks();
            if (co_timeout != null)
            {
                TaskManager.StopCoroutineTask(co_timeout);
                co_timeout = null;
            }
#if UNITY_EDITOR
            TaskMonitor.RecordTaskDone(this);
#endif
        }

        private void SetResult(Result result, ErrorCode errorCode, string error)
        {
            if (this.result != Result.None) return;
            this.result = result;
            this.errorCode = errorCode;
            this.error = error;
        }

        private void NotifyFinishCallbacks()
        {
            if (finishCallbacks == null) return;
            for(int i=0; i<finishCallbacks.Count; ++i)
            {
                var item = finishCallbacks[i];
                if ((item.result & result) == 0) continue;
                if (result == Result.Fail && (item.errorCode & errorCode) == 0) continue;

                item.callback(this);
            }
            finishCallbacks.Clear();
            if(progressCallbacks != null) progressCallbacks.Clear();
        }

        private void NotifyProgressCallbacks()
        {
            if (progressCallbacks == null) return;
            for (int i = 0; i < progressCallbacks.Count; ++i)
            {
                var callback = progressCallbacks[i];
                callback(this, progress);
            }
        }

        protected virtual void OnStart() { }
        
    }

    


#if UNITY_EDITOR
    public class TaskMonitor : Singleton<TaskMonitor>
    {
        public static void RecordTaskCreate(Task task)
        {
            instance.runtimeTaskList.Add(task);
        }

        public static void RecordTaskStart(Task task)
        {
            instance.runtimeTaskList.Add(task);
        }

        public static void RecordTaskDone(Task task)
        {
            instance.runtimeTaskList.Remove(task);
        }

        public static HashSet<Task> GetRuntimeInfo()
        {
            return instance.runtimeTaskList;
        }

        protected HashSet<Task> runtimeTaskList = new HashSet<Task>();
    }
#endif
}

