using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using UnityEngine.EventSystems;

namespace QuickUnity
{
    public interface IAnimatorStateEnterHandler : IEventSystemHandler
    {
        void OnAnimatorStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller);
    }

    public class AnimatorStateEnter : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            ExecuteEvents.Execute<IAnimatorStateEnterHandler>(
                target: animator.gameObject,
                eventData: null,
                functor: (handler, data) => handler.OnAnimatorStateEnter(animator, stateInfo, layerIndex, controller)
            );
        }
    }
}


