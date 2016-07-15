using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using UnityEngine.EventSystems;

namespace QuickUnity
{
    public interface IAnimatorStateStateIKHandler : IEventSystemHandler
    {
        void OnAnimatorStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller);
    }

    public class AnimatorStateStateIK : StateMachineBehaviour
    {
        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            ExecuteEvents.Execute<IAnimatorStateStateIKHandler>(
                target: animator.gameObject,
                eventData: null,
                functor: (handler, data) => handler.OnAnimatorStateIK(animator, stateInfo, layerIndex, controller)
            );
        }
    }
}


