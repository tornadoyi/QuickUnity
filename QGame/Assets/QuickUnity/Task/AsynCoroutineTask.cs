using UnityEngine;
using System.Collections;
using System;


namespace QuickUnity
{
    /// <summary>
    /// AsynCoroutineTask means the job will be Executed in a thread(based Asynchronous Programming Model)
    /// In Coroutine, check the status of asynchronous job and call appropriate event. 
    /// This means, the real job is in other thread and can't access u3d's object , but all event handler will accept event in u3d's main thread. 
    /// Note!!!  Now,the task's caller must in U3d's main thread. Maybe improve later. 
    /// Note!!!!!  In Test, Maybe U3d use ThreadPool too, So asynRun may run in same thread as U3d's main loop, And froze U3d's main loop.
    /// </summary>
    public class AsynCoroutineTask : Task
    {
        #region Member Variables

        delegate void runDelegate();

        IAsyncResult m_asynRet;
        runDelegate m_asynDelegate;

        #endregion

        #region Method
        protected override void OnStart()
        {
            if (m_asynDelegate != null || m_asynRet != null)
            {
                SetFinish();
                return;
            }

            m_asynDelegate = new runDelegate(this.OnProcess);
            try
            {
                m_asynRet = m_asynDelegate.BeginInvoke(null, null);
                TaskManager.CoroutineTask(new TaskManager.CoroutineTaskDelegate(this.CoroutineMethod));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

        }

        protected virtual void OnProcess() { throw new System.InvalidOperationException("Not Implement"); }

        IEnumerator CoroutineMethod()
        {
            if (m_asynRet == null)
            {
                Debug.LogError("m_asynRet == null");
                yield return null;
            }

            while (!m_asynRet.IsCompleted)
            {
                yield return null;
            }

            try
            {
                m_asynDelegate.EndInvoke(m_asynRet);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            // Finish,  Note: Must be the end of this function
            SetFinish();
        }

        #endregion
    }
}



