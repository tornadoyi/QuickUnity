using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class QuickBehaviour : MonoBehaviour
    {
        public Action.ActionBase Schedule(float time, Action.CallFunc.Callback callback)
        {
            return action.Schedule(time, callback);
        }

        public Action.ActionBase ScheduleOnce(float time, Action.CallFunc.Callback callback)
        {
            return action.ScheduleOnce(time, callback);
        }

        public void UnSchedule(Action.ActionBase action)
        {
            this.action.UnSchedule(action);
        }

//         public bool RegisterEventDelegate(EventRouter.EventDelegate eventDelegate, params int[] events)
//         {
//             return eventRouter.RegisterDelegate(eventDelegate, events);
//         }
// 
//         public void UnregisterEventDelegate(EventRouter.EventDelegate eventDelegate)
//         {
//             eventRouter.UnregisterDelegate(eventDelegate);
//         }
// 
//         public void SendEvent(int eventID, params System.Object[] parms)
//         {
//             eventRouter.SendEvent(eventID, parms);
//         }
// 
//         public void BroadcastEvent(int eventID, params System.Object[] parms)
//         {
//             eventRouter.BroadcastEvent(eventID, parms);
//         }




        protected T AddBehaviour<T>(ref T parm) where T : MonoBehaviour
        {
            if (parm != null) return parm;
            parm = GetComponent<T>();
            if (parm != null) return parm;
            parm = gameObject.AddComponent<T>();
            return parm;
        }

        protected T AddModel<T>(ref T parm) where T : new()
        {
            if (parm != null) return parm;
            parm = new T();
            return parm;
        }

        // Action
        public Action action { get { return AddBehaviour<Action>(ref _action); } }
        private Action _action;

        // Event
        //public EventRouter eventRouter { get { return AddBehaviour<EventRouter>(ref _eventRouter); } }
        //private EventRouter _eventRouter;
    }
}

