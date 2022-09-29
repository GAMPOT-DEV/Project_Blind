using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class MeleeAttackCombo4SMB: SceneLinkedSMB<PlayerCharacter>
    {
        UI_FieldScene ui = null;
        private bool _powerAttack = false;
        private bool _checkForPowerAttack = false;
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.ReAttackSize(6, 4, _monoBehaviour.Data.damage + 2);
            _monoBehaviour.StopMoveY();
            if(!_monoBehaviour.isPowerAttack) SoundManager.Instance.Play("Player/Attack/5타(수정)", Define.Sound.Effect);
        }

        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_monoBehaviour.isPowerAttack)
            {
                animator.speed = 0.06f;
                _checkForPowerAttack = true;
                _monoBehaviour.EndPowerAttack();
                if (ui == null)
                {
                    ui = FindObjectOfType<UI_FieldScene>();
                }
                if (ui != null)
                {
                    ui.StartCharge();
                }
            }
            else
            {
                if (_monoBehaviour.isJump)
                {
                    _monoBehaviour.AttackableMove(_monoBehaviour.Data.attackMove * (float)_monoBehaviour.GetFacing());
                }
                _monoBehaviour.PlayAttackFx(3,_monoBehaviour.GetFacing());
                _monoBehaviour.enableAttack();
            }
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.UpdateVelocity();
            
            if (!_monoBehaviour.isJump)
            {
                _monoBehaviour.AirborneVerticalMovement(1f);
                _monoBehaviour.UpdateJump();
                _monoBehaviour.CheckForGrounded();
                _monoBehaviour.GroundedHorizontalMovement(true);
                _monoBehaviour.UpdateFacing();
            }
            else _monoBehaviour.GroundedHorizontalMovement(false);
            
            if(_monoBehaviour.CheckForDeed())
            {
                _monoBehaviour.Deed();
            }
            if (_monoBehaviour.CheckForDash())
            {
                _monoBehaviour.DashStart();
            }
            
            if(_monoBehaviour.CheckForParing())
                _monoBehaviour.Paring();

            
            if ((_monoBehaviour.isPowerAttackEnd &&!_powerAttack && _monoBehaviour.CurrentWaveGauge >= 10)) 
            {
                animator.speed = 1.0f;
                _monoBehaviour._attack.DamageReset(_monoBehaviour.Data.powerAttackdamage);
                _monoBehaviour.enableAttack();
                _monoBehaviour.AttackableMove(_monoBehaviour.Data.attackMove * (float)_monoBehaviour.GetFacing());
                _monoBehaviour.CurrentWaveGauge -= 10;
                _monoBehaviour.isPowerAttackEnd = false;
                _monoBehaviour.PlayAttackFx(7,_monoBehaviour.GetFacing());

                if (ui == null)
                {
                    ui = FindObjectOfType<UI_FieldScene>();
                }
                if (ui != null)
                {
                    ui.StopCharge();
                }
                _powerAttack = true;
                _checkForPowerAttack = false;
            }
        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour._attack.DefultDamage();
            _monoBehaviour.DisableAttack();
            _monoBehaviour.MeleeAttackComoEnd();
            _powerAttack = false;
        }
    }
}