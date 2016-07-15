using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace QuickUnity
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CollisionEventTrigger), true)]
    public class CollisionEventTriggerInspector : EventTriggerInspector
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            events = new int[] {
                (int)EngineEvent.CollisionEnter,
                (int)EngineEvent.CollisionExit,
                (int)EngineEvent.TriggerEnter,
                (int)EngineEvent.TriggerExit,
                (int)EngineEvent.CollisionEnter2D,
                (int)EngineEvent.CollisionExit2D,
                (int)EngineEvent.TriggerEnter2D,
                (int)EngineEvent.TriggerExit2D
            };
        }

        protected override string OnEventToString(int id)
        {
            var e = (EngineEvent)id;
            return e.ToString();
        }

    }
}


