using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using UnityEngine.EventSystems;

namespace QuickUnity
{
    public interface IAnimatorStateMoveHandler : IEventSystemHandler
    {
        void OnAnimatorStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller);
    }

    public class AnimatorStateMove : StateMachineBehaviour
    {
        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            ExecuteEvents.Execute<IAnimatorStateMoveHandler>(
                target: animator.gameObject,
                eventData: null,
                functor: (handler, data) => handler.OnAnimatorStateMove(animator, stateInfo, layerIndex, controller)
            );
        }
    }
}

