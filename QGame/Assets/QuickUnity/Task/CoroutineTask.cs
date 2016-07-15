using UnityEngine;
using System.Collections;


namespace QuickUnity
{
    /// <summary>
    /// CoroutineTask means a job which will be executed in Coroutine, And the caller will not be frozen.
    /// In fact, the CoroutineTask also be executed in U3d's main thread. 
    /// It's a wrapper for unity's coroutine mechanism. 
    /// Note!!!  Now,the task's caller must in U3d's main thread. Maybe improve later. 
    /// </summary>
    public class CoroutineTask : Task
    {
        protected override void OnStart()
        {
            method = OnProcess();
            TaskManager.CoroutineTask(new TaskManager.CoroutineTaskDelegate(this.StateController));
        }

        protected virtual IEnumerator OnProcess()
        {
            throw new System.InvalidOperationException("Not Implement");
        }

        protected IEnumerator StateController()
        {
            yield return null;

            IEnumerator e = method;

            while (!done)
            {
                if (e != null && e.MoveNext())
                {
                    yield return e.Current;
                }
                else
                {
                    break;
                }
            }

            Done();
        }

        private IEnumerator method;
    }
}
