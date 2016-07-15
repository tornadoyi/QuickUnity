using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using SLua;

namespace QuickUnity
{
    public class EventManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            for (int i = 0; i < archiveReceivers.Count; ++i)
            {
                var item = archiveReceivers[i];
                List<Receiver> list = null;
                if (!receiverDict.TryGetValue(item.eventID, out list))
                {
                    list = new List<Receiver>();
                    receiverDict.Add(item.eventID, list);
                }
                list.Add(item);
            }
            //archiveReceivers.Clear();
        }

        public virtual bool RegisterReciver(MonoBehaviour receiver, int eventID, string functionName)
        {
            if( receiver == null || 
                !(receiver is IEventReceiver) ||
                !(receiver is IStandardEventReceiver))
            {
                return false;
            }

            var item = new Receiver(receiver, eventID, functionName);

            List<Receiver> list = null;
            if (!receiverDict.TryGetValue(eventID, out list))
            {
                list = new List<Receiver>();
                receiverDict.Add(eventID, list);
            }
            list.Add(item);

#if UNITY_EDITOR
            if(!Application.isPlaying)
            {
                archiveReceivers.Add(item);
            }
#endif
            return true;
        }

        public void SendEvent(int eventID, System.Object parm)
        {
            //  Check receivers
            List<Receiver> list = null;
            if (!receiverDict.TryGetValue(eventID, out list)) return;

            // Create invalid receiver list
            List<int> invalidList = null;

            // Loop for send
            for (int i=0; i<list.Count; ++i)
            {
                var item = list[i];
                if(item.receiver == null)
                {
                    if (invalidList == null) invalidList = new List<int>();
                    invalidList.Add(i);
                    continue;
                }
                if(item.receiver is IStandardEventReceiver)
                {
                    (item.receiver as IStandardEventReceiver).OnProcessEvent(eventID, parm, item.functionName, this);
                }
                else if(item.receiver is IEventReceiver)
                {
                    (item.receiver as IEventReceiver).OnProcessEvent(eventID, parm, item.functionName, this);
                }
            }

            // Clean invalid
            if(invalidList != null)
            {
                for(int i=invalidList.Count-1; i>=0; --i)
                {
                    var index = invalidList[i];
                    list.RemoveAt(index);
                }
            }
        }

        public void SendEventAsync(int eventID, System.Object parm)
        {
            delaySendEventList.Add(new EventItem(eventID, parm));
            if (co != null) return;
            co = StartCoroutine(SendCacheEvents());
        }

        protected IEnumerator SendCacheEvents()
        {
            for(int i=0; i<delaySendEventList.Count; ++i)
            {
                var item = delaySendEventList[i];
                SendEvent(item.eventID, item.parm);
            }
            delaySendEventList.Clear();
            co = null;
            yield break;
        }

        public static bool CheckReceiverValid(MonoBehaviour mb)
        {
            if (mb == null) return false;
            return (mb is IStandardEventReceiver ||
                    mb is IEventReceiver);

        }


#if UNITY_EDITOR
        [DoNotToLua]
        public List<Receiver> GetArchiveReceivers() { return archiveReceivers; }
#endif


        // Coroutine for send async
        protected Coroutine co;

        // Event cache for delay
        protected List<EventItem> delaySendEventList = new List<EventItem>();

        // Runtime event receivers
        protected Dictionary<int, List<Receiver>> receiverDict = new Dictionary<int, List<Receiver>>();

        [SerializeField]
        protected List<Receiver> archiveReceivers = new List<Receiver>();


        protected struct EventItem
        {
            public EventItem(int eventID, System.Object parm)
            {
                this.eventID = eventID;
                this.parm = parm;
            }
            public int eventID { get; private set; }
            public System.Object parm { get; private set; }
        }

        [System.Serializable]
#if UNITY_EDITOR
        public class Receiver
#else
        protected class Receiver
#endif
        {
            public Receiver(MonoBehaviour receiver, int eventID, string functionName)
            {
                this.receiver = receiver;
                this.eventID = eventID;
                this.functionName = functionName;
            }

            public MonoBehaviour receiver;
            public string functionName = string.Empty;
            public int eventID;
            
        }
    }


    public interface IEventReceiver
    {
        void OnProcessEvent(int eventID, System.Object parm, string functionName, EventManager source);
    }


    public struct StandardEventData
    {
        public StandardEventData(System.Object parm, EventManager source)
        {
            this.parm = parm;
            this.source = source;
        }
        public System.Object parm { get; private set; }
        public EventManager source { get; private set; }
    }
    public interface IStandardEventReceiver { GameObject gameOject { get; } }
    public static class IStandardEventReceiver_Ext
    {
        public static void OnProcessEvent(this IStandardEventReceiver self, int eventID, System.Object parm, string functionName, EventManager source)
        {
            if(string.IsNullOrEmpty(functionName))
            {
                Debug.LogErrorFormat("Can not send event {0}, because Invalid function name", eventID);
                return;
            }
            self.gameOject.SendMessage(functionName, new StandardEventData(parm, source), SendMessageOptions.DontRequireReceiver);
        }
    }


    public enum EngineEvent
    {
        // UI
        PointerEnter                = EventTriggerType.PointerEnter,
        PointerExit                 = EventTriggerType.PointerExit,
        PointerDown                 = EventTriggerType.PointerDown,
        PointerUp                   = EventTriggerType.PointerUp,
        PointerClick                = EventTriggerType.PointerClick,
        Drag                        = EventTriggerType.Drag,
        Drop                        = EventTriggerType.Drop,
        Scroll                      = EventTriggerType.Scroll,
        UpdateSelected              = EventTriggerType.UpdateSelected,
        Select                      = EventTriggerType.Select,
        Deselect                    = EventTriggerType.Deselect,
        Move                        = EventTriggerType.Move,
        InitializePotentialDrag     = EventTriggerType.InitializePotentialDrag,
        BeginDrag                   = EventTriggerType.BeginDrag,
        EndDrag                     = EventTriggerType.EndDrag,
        Submit                      = EventTriggerType.Submit,
        Cancel                      = EventTriggerType.Cancel,

        // Collision
        CollisionEnter              = -1000,
        CollisionExit               = -1001,
        TriggerEnter                = -1002,
        TriggerExit                 = -1003,
        CollisionEnter2D            = -1004,
        CollisionExit2D             = -1005,
        TriggerEnter2D              = -1006,
        TriggerExit2D               = -1007,
    }
}