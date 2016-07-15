using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class CustomTask : Task
    {
        protected override void OnStart()
        {
            // Nothing to do
        }

        public void SetResultFailed(string error)
        {
            base.SetResultFailed(error);
        }

        new public void Done()
        {
            base.Done();
        }

        new public void Error(string error)
        {
            base.Error(error);
        }
    }
}

