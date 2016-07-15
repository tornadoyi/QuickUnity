using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLua;

namespace QuickUnity
{
    public class EventTrigger : EventManager
    {
        protected override void Awake()
        {
            base.Awake();
            for(int i=0; i<events.Count; ++i)
            {
                eventDict.Add(events[i].eventID, events[i]);
            }
            //eventDict.Clear();
        }


        protected void ProccessEvent(int eventId, System.Object parm = null)
        {
            EventInfo info = null;
            if(!eventDict.TryGetValue(eventId, out info))
            {
                return;
            }
            if(info.mode == EventSendMode.Imimmediately)
            {
                SendEvent(eventId, parm);
            }
            else
            {
                SendEventAsync(eventId, parm);
            }
        }


#if UNITY_EDITOR
        [DoNotToLua]
        public void AddEvent(int eventID, EventSendMode mode)
        {
            if (HasEventRegistered(eventID)) return;
            events.Add(new EventInfo(eventID, mode));
        }

        [DoNotToLua]
        public bool HasEventRegistered(int eventID)
        {
            for (int i = 0; i < events.Count; ++i)
            {
                var item = events[i];
                if (item.eventID == eventID) return true;
            }
            return false;
        }
#endif

        protected Dictionary<int, EventInfo> eventDict = new Dictionary<int, EventInfo>();
        public List<EventInfo> events = new List<EventInfo>();


        [System.Serializable]
        public class EventInfo
        {
            public EventInfo(int eventID, EventSendMode mode)
            {
                this.eventID = eventID;
                this.mode = mode;
            }
            public int eventID;
            public EventSendMode mode;
        }

        public enum EventSendMode
        {
            Imimmediately = 0,
            Delay,
        }
    }


    
}
