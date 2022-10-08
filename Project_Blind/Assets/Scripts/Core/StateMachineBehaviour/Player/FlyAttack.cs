using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class FlyAttack: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.enableAttack();

            SoundManager.Instance.Play("Player/Attack/1타", Define.Sound.Effect);
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            
            _monoBehaviour.CheckForGrounded();
            _monoBehaviour.GroundedHorizontalMovement(true);
            _monoBehaviour.AirborneVerticalMovement(_monoBehaviour.gravity);
            _monoBehaviour.UpdateJump();
            _monoBehaviour.UpdateVelocity();

        }
        
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.MeleeAttackComoEnd();
            //Debug.Log(_monoBehaviour._clickcount);
            _monoBehaviour._attack.DefultDamage();  
            _monoBehaviour.DisableAttack();
        }
    }
}