using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using UnityEngine.EventSystems;

namespace QuickUnity
{
    public interface IAnimatorStateMachineEnterHandler : IEventSystemHandler
    {
        void OnStateStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller);
    }

    public class AnimatorStateMachineEnter : StateMachineBehaviour
    {
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
        {
            ExecuteEvents.Execute<IAnimatorStateMachineEnterHandler>(
                target: animator.gameObject,
                eventData: null,
                functor: (handler, data) => handler.OnStateStateMachineEnter(animator, stateMachinePathHash, controller)
            );
        }
    }
}

