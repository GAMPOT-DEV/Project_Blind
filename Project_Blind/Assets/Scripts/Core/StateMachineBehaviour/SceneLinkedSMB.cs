using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    /// <summary>
    /// 모든 상태머신들의 부모 클래스입니다.
    /// 2DGameKit에 있는 클래스를 가져왔습니다.
    /// </summary>
    /// <typeparam name="TMonoBehaviour">상태머신을 정의한 캐릭터의 동작 클래스입니다. 예) PlayerCharacter</typeparam>
    public class SceneLinkedSMB<TMonoBehaviour> : SealedSMB
        where TMonoBehaviour : MonoBehaviour
    {
        protected TMonoBehaviour _monoBehaviour;

        private bool _firstFrameHappened;
        private bool _lastFrameHappened;
        
        /// <summary>
        /// 모든 상태머신들을 초기화합니다.
        /// </summary>
        /// <param name="animator">캐릭터의 애니메이터를 불러옵니다.</param>
        /// <param name="monoBehaviour"> 상태머신을 정의한 캐릭터의 동작 클래스입니다.</param>
        public static void Initialise(Animator animator, TMonoBehaviour monoBehaviour)
        {
            SceneLinkedSMB<TMonoBehaviour>[] sceneLinkedSMBs =
                animator.GetBehaviours<SceneLinkedSMB<TMonoBehaviour>>();

            for (int i = 0; i < sceneLinkedSMBs.Length; i++)
            {
                sceneLinkedSMBs[i].InternalInitialise(animator, monoBehaviour);
            }
        }

        protected void InternalInitialise(Animator animator, TMonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
            OnStart(animator);
        }

        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _firstFrameHappened = false;

            OnSLStateEnter(animator, stateInfo, layerIndex);
            OnSLStateEnter(animator, stateInfo, layerIndex, controller);
        }
        /// <summary>
        /// 아래 함수들의 실행 조건을 설정하는 함수입니다.
        /// </summary>
        public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (!animator.gameObject.activeSelf)
                return;

            if (animator.IsInTransition(layerIndex) && animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash ==
                stateInfo.fullPathHash)
            {
                OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex);
                OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex, controller);
            }

            if (!animator.IsInTransition(layerIndex) && _firstFrameHappened)
            {
                OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
                OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex, controller);
            }

            if (animator.IsInTransition(layerIndex) && !_lastFrameHappened && _firstFrameHappened)
            {
                _lastFrameHappened = true;

                OnSLStatePreExit(animator, stateInfo, layerIndex);
                OnSLStatePreExit(animator, stateInfo, layerIndex, controller);
            }

            if (!animator.IsInTransition(layerIndex) && !_firstFrameHappened)
            {
                _firstFrameHappened = true;

                OnSLStatePostEnter(animator, stateInfo, layerIndex);
                OnSLStatePostEnter(animator, stateInfo, layerIndex, controller);
            }

            if (animator.IsInTransition(layerIndex) &&
                animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash == stateInfo.fullPathHash)
            {
                OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex);
                OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex, controller);
            }
        }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _lastFrameHappened = false;

            OnSLStateExit(animator, stateInfo, layerIndex);
            OnSLStateExit(animator, stateInfo, layerIndex, controller);
        }

        /// <summary>
        /// 동작클래스가 시작되면 호출하는 함수입니다.
        /// </summary>
        public virtual void OnStart(Animator animator)
        {
        }

        /// <summary>
        /// 상태의 트랜지션 상태에서 실행되는 함수입니다.
        /// </summary>
        public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        /// <summary>
        /// 위 함수 실행 후 트랜지션 매 프레임마다 실행되는 함수입니다.
        /// </summary>
        public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        /// <summary>
        /// 위 함수 실행 후 실행되는 함수입니다.
        /// </summary>
        public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        /// <summary>
        /// 위 함수가 실행 됬는데 트랜지션이 끝나지 않았다면 실행되는 함수입니다.
        /// </summary>
        public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        /// <summary>
        /// 트랜지션이 끝나면 실행되는 함수입니다.
        /// </summary>
        public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        /// <summary>
        /// 위 함수가 실행된 후 상태에 들어가기 직전까지 실행되는 함수입니다.
        /// </summary>
        public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        /// <summary>
        /// 상태가 끝나면 실행되는 함수입니다.
        /// </summary>
        public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        /// <summary>
        /// 상태에 진입하면 실행되는 함수입니다.
        /// </summary>
        public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
        }

        /// <summary>
        /// 위 함수가 실행된 후 매 프레임마다 실행되는 함수입니다.
        /// </summary>
        public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex, AnimatorControllerPlayable controller)
        {
        }

        /// <summary>
        /// Called on the first frame after the transition to the state has finished.
        /// </summary>
        public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
        }

        /// <summary>
        /// Called every frame when the state is not being transitioned to or from.
        /// </summary>
        public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex, AnimatorControllerPlayable controller)
        {
        }

        /// <summary>
        /// Called on the first frame after the transition from the state has started.  Note that if the transition has a duration of less than a frame, this will not be called.
        /// </summary>
        public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
        }

        /// <summary>
        /// Called after OnSLStatePreExit every frame during transition to the state.
        /// </summary>
        public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex, AnimatorControllerPlayable controller)
        {
        }

        /// <summary>
        /// Called after Updates when execution of the state first finshes (after transition from the state).
        /// </summary>
        public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
        }
    }

    /// <summary>
    /// 이 클래스는 기본 StateMachineBehaviour를 대체하는 클래스입니다.
    /// </summary>
    public abstract class SealedSMB : StateMachineBehaviour
    {
        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}