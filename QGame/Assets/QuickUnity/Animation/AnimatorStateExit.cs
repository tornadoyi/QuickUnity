using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using UnityEngine.EventSystems;

namespace QuickUnity
{
    public interface IAnimatorStateExitHandler : IEventSystemHandler
    {
        void OnAnimatorStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller);
    }

    public class AnimatorStateExit : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            ExecuteEvents.Execute<IAnimatorStateExitHandler>(
                target: animator.gameObject,
                eventData: null,
                functor: (handler, data) => handler.OnAnimatorStateExit(animator, stateInfo, layerIndex, controller)
            );
        }
    }
}

