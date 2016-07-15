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
        
        public delegate IEnumerator CoroutineTaskDelegate();
    }



    /// <summary>
    /// Task means a job which will be executed in Coroutine Or Thread or Immediately, depend on implementation . 
    /// This is just a interface.
    /// </summary>
    public class Task
    {
        public void Start() { Start(null, null); }
        public void Start(TaskDoneHandler doneCallback, TaskProcessHandler progressCallback)
        {
            if (state != TaskState.Ready) { Debug.LogError("Task has been used, cannot start again"); return; }
            state = TaskState.Running;
            doneHandler += doneCallback;
            progressHandler += progressCallback;
            _startTime = Time.time;
            OnStart();
            if(_timeout >= 0)
            {
                co_timeout = TaskManager.CoroutineTask(new TaskManager.CoroutineTaskDelegate(WaitForTimeout));
            }
#if UNITY_EDITOR
            TaskMonitor.RecordTaskStart(this);
#endif
        }

        public IEnumerator StartAndWaitForDone(TaskDoneHandler doneCallback = null, TaskProcessHandler progressCallback = null)
        {
            Start(doneCallback, progressCallback);
            return WaitForDone();
        }

        public IEnumerator WaitForDone()
        {
            while (!done)
            {
                yield return null;
            }
        }

        protected IEnumerator WaitForTimeout()
        {
            yield return new WaitForSeconds(_timeout);
            if (state == TaskState.Done) yield break;
            SetResultFailed(string.Format("Task is timeout, timeout:{0}", _timeout));
            Done();
        }

        protected virtual void OnStart() { }

        protected void Done()
        {
            End();
        }

        protected void Error(string error)
        {
            _result = false;
            SetError(error);
            End();
        }

        private void End()
        {
            if (state == TaskState.Done) return;
            state = TaskState.Done;
            _endTime = Time.time;
            SendDoneMessage();
            if (co_timeout != null)
            {
                TaskManager.StopCoroutineTask(co_timeout);
                co_timeout = null;
            }
#if UNITY_EDITOR
            TaskMonitor.RecordTaskDone(this);
#endif
        }

        protected virtual void Progress(float percent)
        {
            if (state != TaskState.Running || percent <= _progress) return;
            _progress = percent;
            SendProcessMessage();
        }

        protected virtual void SetResultFailed(System.Exception e) { _result = false; SetError(e.ToString()); }
        protected virtual void SetResultFailed(string error, string suberror = default(string))
        {
            _result = false;
            SetError(error);
            SetError(suberror);
        }

        private void SetError(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;
            if (string.IsNullOrEmpty(_error))
            {
                _error = msg;
            }
            else
            {
                _error += "\n" + msg;
            }
        }

        protected virtual void SetResultSucess() { _result = true; }

        private void SendDoneMessage()
        {
            if (doneHandler != null) doneHandler.Invoke(result);
        }

        private void SendProcessMessage()
        {
            if (progressHandler != null) progressHandler.Invoke(progress);
        }

        public void AddDoneCallback(TaskDoneHandler handler) { doneHandler += handler; }
        public void AddProgressCallback(TaskProcessHandler handler) { progressHandler += handler; }

        public event TaskDoneHandler doneHandler;
        public event TaskProcessHandler progressHandler;

        public float progress { get { return _progress; } }
        private float _progress = 0;

        public bool result { get { return _result; } }
        private bool _result = true;

        public string error { get { return _error; } }
        private string _error = string.Empty;

        public bool ready { get { return state == TaskState.Ready; } }
        public bool running { get { return state == TaskState.Running; } }
        public bool done { get { return state == TaskState.Done; } }

        public float lastTime
        {
            get
            {
                if (_startTime == 0) return 0;
                if (_endTime == 0) return Time.time - _startTime;
                return _endTime - _startTime;
            }
        }

        public float startTime { get { return _startTime; } }
        private float _startTime = 0;

        public float endTime { get { return _endTime; } }
        private float _endTime = 0;

        public float timeout
        {
            get { return _timeout; }
            set
            {
                if(value < 0) { Debug.LogErrorFormat("Invalid timeout {0}", value); return; }
                if(state != TaskState.Ready)
                {
                    Debug.LogError("Can not set timeout after task run");
                    return;
                }
                _timeout = value;
            }
        }
        protected float _timeout = -1;
        private Coroutine co_timeout;

        protected bool hasTimeout
        {
            get
            {
                if (_timeout < 0) return false;
                return (lastTime - _startTime >= _timeout);
            }
        }

        public TaskState state { get; private set; }

        public enum TaskState
        {
            Ready = 0,
            Running,
            Done,
        }
    }

    public delegate void TaskDoneHandler(bool result = true);
    public delegate void TaskProcessHandler(float percent = 100.0f);


#if UNITY_EDITOR
    public class TaskMonitor : Singleton<TaskMonitor>
    {
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

