using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using UnityEngine.EventSystems;

namespace QuickUnity
{
    public interface IAnimatorStateMachineExitHandler : IEventSystemHandler
    {
        void OnStateStateMachineExit(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller);
    }

    public class AnimatorStateMachineExit : StateMachineBehaviour
    {
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
        {
            ExecuteEvents.Execute<IAnimatorStateMachineExitHandler>(
                target: animator.gameObject,
                eventData: null,
                functor: (handler, data) => handler.OnStateStateMachineExit(animator, stateMachinePathHash, controller)
            );
        }
    }
}


