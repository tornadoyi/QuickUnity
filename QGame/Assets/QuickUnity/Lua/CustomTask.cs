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

        new public void SetSuccess() { base.SetSuccess(); SetFinish(); }
        new public void SetFail(string error) { base.SetFail(error); SetFinish(); }
        new public void SetCancel(string error = default(string)) { base.SetCancel(error); SetFinish(); }
        new public void SetTimeout(string error = default(string)) { base.SetTimeout(error); SetFinish(); }
    }
}

