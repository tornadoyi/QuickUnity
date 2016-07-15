using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace QuickUnity
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EventTrigger), true)]
    public class EventTriggerInspector : Editor
    {
        protected virtual void OnEnable()
        {
            component = target as EventTrigger;

            RefreshArchiveReceiverDictionary();
        }

        public override void OnInspectorGUI()
        {
            if (dirty)
            {
                RefreshArchiveReceiverDictionary();
                dirty = false;
            }

            // Sorts
            var sortList = new List<KeyValuePair<EventTrigger.EventInfo, List<EventTrigger.Receiver>>>();
            foreach (var it in dict)
            {
                sortList.Add(it.Value);
            }
            sortList.Sort((x, y) =>{ return  x.Key.eventID - y.Key.eventID; });

            // Show register events
            foreach (var it in sortList)
            {
                var eventInfo = it.Key;
                var list = it.Value;

                using (QuickEditor.BeginVertical("GroupBox"))
                {
                    ShowEventList(eventInfo, list);
                }
            }


            // Show add button
            GUILayout.Space(10);
            using (QuickEditor.BeginHorizontal())
            {
                ShowAddNewEventTypeButton();
            }
            
            if (GUI.changed) EditorUtility.SetDirty(target);

        }

        protected void ShowEventList(EventTrigger.EventInfo eventInfo, List<EventTrigger.Receiver> list)
        {
            using (QuickEditor.BeginHorizontal())
            {
                EditorGUILayout.LabelField( OnEventToString(eventInfo.eventID), GUILayout.Width(150));
                eventInfo.mode = (EventTrigger.EventSendMode)EditorGUILayout.EnumPopup(eventInfo.mode, GUILayout.Width(100));

                GUILayout.Label("");

                if (GUILayout.Button("+", "minibutton", GUILayout.Width(20)))
                {
                    var archiveList = component.GetArchiveReceivers();
                    archiveList.Add(new EventManager.Receiver(null, eventInfo.eventID, string.Empty));
                    dirty = true;
                }

                if (GUILayout.Button("X", "minibutton", GUILayout.Width(20)))
                {
                    component.events.Remove(eventInfo);
                    var archiveList = component.GetArchiveReceivers();
                    for(int i=archiveList.Count-1; i>=0; --i)
                    {
                        if (archiveList[i].eventID != eventInfo.eventID) continue;
                        archiveList.RemoveAt(i);
                    }
                    dirty = true;
                }
            }

            foreach (var receiver in list)
            {
                GUILayout.Space(15);
                ShowReceiver(receiver);
            }
        }

        protected void ShowReceiver(EventTrigger.Receiver receiver)
        {
            using (QuickEditor.BeginVertical())
            {
                using (QuickEditor.BeginHorizontal())
                {
                    // Reset 
                    if(receiver.receiver != null && !EventManager.CheckReceiverValid(receiver.receiver))
                    {
                        Debug.LogError("Event receiver must inherit IEventReceiver or IStandardEventReceiver");
                        receiver.receiver = null;
                    }

                    // Find property mono behavior
                    var mb = EditorGUILayout.ObjectField(receiver.receiver, typeof(MonoBehaviour), true) as MonoBehaviour;
                    if(mb != null)
                    {
                        if (mb is IEventReceiver || mb is IStandardEventReceiver)
                        {
                            receiver.receiver = mb;
                        }
                        else
                        {
                            var mbs = mb.gameObject.GetComponents<MonoBehaviour>();
                            bool find = false;
                            foreach (var item in mbs)
                            {
                                if (item is IEventReceiver || item is IStandardEventReceiver)
                                {
                                    receiver.receiver = item;
                                    find = true;
                                    break;
                                }
                            }
                            if (!find) { Debug.LogError("Event receiver must inherit IEventReceiver or IStandardEventReceiver"); }
                        }
                    }
                    
                    if (GUILayout.Button("-", GUILayout.Width(20)))
                    {
                        var archiveList = component.GetArchiveReceivers();
                        archiveList.Remove(receiver);
                        dirty = true;
                    }
                }

                receiver.functionName = EditorGUILayout.TextField(receiver.functionName);
            }
        }

        protected void ShowAddNewEventTypeButton()
        {
            // Format
            GUILayout.Label("");

            if (GUILayout.Button("Add New Event Type", GUILayout.Width(200), GUILayout.Height(20)))
            {
                var menu = new GenericMenu();
                for(int i=0; i<events.Length; ++i)
                {
                    var e = events[i];
                    var eventName = OnEventToString(e);
                    bool exsit = (target as EventTrigger).HasEventRegistered((int)e);
                    if(!exsit)
                    {
                        menu.AddItem(new GUIContent(eventName), false, AddEvent, e);
                    }
                    else
                    {
                        menu.AddDisabledItem(new GUIContent(eventName));
                    }
                    
                }
                menu.ShowAsContext();
            }

            // Format
            GUILayout.Label("");
        }

        protected virtual void AddEvent(object obj)
        {
            var e = (int)obj;
            component.AddEvent(e, EventTrigger.EventSendMode.Imimmediately);
            dirty = true;
        }

        protected void RefreshArchiveReceiverDictionary()
        {
            dict = new Dictionary<int, KeyValuePair<EventTrigger.EventInfo, List<EventTrigger.Receiver>>>();

            foreach(var info in component.events)
            {
                var list = new List<EventTrigger.Receiver>();
                var pair = new KeyValuePair<EventTrigger.EventInfo, List<EventTrigger.Receiver>>(info, list);
                dict.Add(info.eventID, pair);
            }

            foreach(var receiver in component.GetArchiveReceivers())
            {
                if(!dict.ContainsKey(receiver.eventID))
                {
                    var list = new List<EventTrigger.Receiver>();
                    var info = new EventTrigger.EventInfo(receiver.eventID, EventTrigger.EventSendMode.Imimmediately);
                    var pair = new KeyValuePair<EventTrigger.EventInfo, List<EventTrigger.Receiver>>(info, list);
                    dict.Add(info.eventID, pair);
                }

                var receiverList = dict[receiver.eventID].Value;
                receiverList.Add(receiver);
            }
        }

        protected virtual string OnEventToString(int id)
        {
            return id.ToString();
        }


        protected int[] events = new int[0];

        protected EventTrigger component;

        protected Dictionary<int, KeyValuePair<EventTrigger.EventInfo, List<EventTrigger.Receiver>> > dict;

        protected bool dirty = false;
    }
}


