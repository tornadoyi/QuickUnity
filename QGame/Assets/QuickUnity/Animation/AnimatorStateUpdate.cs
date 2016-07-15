using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using UnityEngine.EventSystems;

namespace QuickUnity
{
    public interface IAnimatorStateUpdateHandler : IEventSystemHandler
    {
        void OnAnimatorStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller);
    }

    public class AnimatorStateUpdate : StateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            ExecuteEvents.Execute<IAnimatorStateUpdateHandler>(
                target: animator.gameObject,
                eventData: null,
                functor: (handler, data) => handler.OnAnimatorStateUpdate(animator, stateInfo, layerIndex, controller)
            );
        }
    }
}

