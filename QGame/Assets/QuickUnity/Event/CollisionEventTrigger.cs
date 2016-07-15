using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class CollisionEventTrigger : EventTrigger
    {
        void OnCollisionEnter(Collision collision)
        {
            ProccessEvent((int)EngineEvent.CollisionEnter, collision);
        }

        void OnCollisionExit(Collision collision)
        {
            ProccessEvent((int)EngineEvent.CollisionExit, collision);
        }

        void OnTriggerEnter(Collider other)
        {
            ProccessEvent((int)EngineEvent.TriggerEnter, other);
        }

        void OnTriggerExit(Collider other)
        {
            ProccessEvent((int)EngineEvent.TriggerExit, other);
        }


        void OnCollisionEnter2D(Collision2D collision)
        {
            ProccessEvent((int)EngineEvent.CollisionEnter2D, collision);
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            ProccessEvent((int)EngineEvent.CollisionExit2D, collision);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            ProccessEvent((int)EngineEvent.TriggerEnter2D, other);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            ProccessEvent((int)EngineEvent.TriggerExit2D, other);
        }
    }
}


