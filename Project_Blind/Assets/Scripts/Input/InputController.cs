using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 입력을 받는 매니저입니다.
    /// 키 입력을 할당하고 어떤 키가 어떤 상태인지 확인할 수 있습니다.
    /// </summary>
    public class InputController : InputComponent<InputController>
    {
        // 초기 키 저장하는 딕셔너리
        public Dictionary<Define.KeyAction, KeyCode> InitialKeyActions
        {
            get;
            private set;
        } = new Dictionary<Define.KeyAction, KeyCode>();

        public Dictionary<Define.KeyAction, KeyCode> CurrKeyActions = new Dictionary<Define.KeyAction, KeyCode>();

        protected bool _HaveControl = true;

        protected bool _DebugMenuIsOpen = false;
        // <키들을 정의하는 공간입니다>
        public InputButton Jump;
        public InputButton Dash;
        public InputButton Wave;
        public InputButton Paring;
        public InputButton Interaction;
        public InputButton Attack;
        public InputButton Skill;
        public InputButton ItemT;


        public InputAxis Horizontal;
        public InputAxis Vertical;
        // </키들을 정의하는 공간입니다>
        
        protected override void Awake()
        {
            base.Awake();
            // 조작키를 할당합니다.
            Jump = new InputButton(KeyCode.Space,XboxControllerButtons.A);
            Paring = new InputButton(KeyCode.K, XboxControllerButtons.Leftstick);
            Interaction = new InputButton(KeyCode.N, XboxControllerButtons.X);
            Attack = new InputButton(KeyCode.J, XboxControllerButtons.RightBumper);
            Dash = new InputButton(KeyCode.Space, XboxControllerButtons.LeftBumper);
            Wave = new InputButton(KeyCode.LeftControl,XboxControllerButtons.B);
            Horizontal = new InputAxis(KeyCode.D, KeyCode.A, XboxControllerAxes.LeftstickHorizontal);
            Vertical = new InputAxis(KeyCode.W,KeyCode.S,XboxControllerAxes.LeftstickVertical);
            Skill = new InputButton(KeyCode.I, XboxControllerButtons.LeftBumper);
            ItemT = new InputButton(KeyCode.Q, XboxControllerButtons.Y);
        }

        /// <summary>
        /// 입력을 가져오는 함수입니다.
        /// </summary>
        /// <param name="fixedUpdateHappened"></param>
        protected override void GetInputs(bool fixedUpdateHappened)
        {
            _HaveControl = true;
            
            Jump.Get(fixedUpdateHappened,inputType);
            Paring.Get(fixedUpdateHappened, inputType);
            Interaction.Get(fixedUpdateHappened, inputType);
            Dash.Get(fixedUpdateHappened, inputType);
            Attack.Get(fixedUpdateHappened, inputType);
            Wave.Get(fixedUpdateHappened,inputType);
            Horizontal.Get(inputType);
            Vertical.Get(inputType);
            Skill.Get(fixedUpdateHappened, inputType);
            ItemT.Get(fixedUpdateHappened, inputType);
        }
        /// <summary>
        /// 막은 키 입력을 다시 활성화 합니다.
        /// </summary>
        /// <param name="resetValues"></param>

        public override void GainControl()
        {
            _HaveControl = true;
            
            GainControl(Jump);
            GainControl(Paring);
            GainControl(Interaction);
            GainControl(Horizontal);
            GainControl(Vertical);
            GainControl(Dash);
            GainControl(Wave);
            GainControl(Attack);
            GainControl(Skill);
            GainControl(ItemT);
        }
        
        /// <summary>
        /// 플레이어의 키 입력을 막습니다.
        /// </summary>
        /// <param name="resetValues"></param>

        public override void ReleaseControl(bool resetValues = true)
        {
            _HaveControl = false;
            
            ReleaseControl(Jump,resetValues);
            ReleaseControl(Paring, resetValues);
            ReleaseControl(Interaction, resetValues);
            ReleaseControl(Horizontal,resetValues);
            ReleaseControl(Vertical,resetValues);
            ReleaseControl(Dash,resetValues);
            ReleaseControl(Wave,resetValues);
            ReleaseControl(Attack, resetValues);
            ReleaseControl(Skill, resetValues);
            ReleaseControl(ItemT, resetValues);
        }

        public void ReKetSet(string key, KeyCode keycode)
        {
            switch (key)
            {
                case "Jump":
                    Jump = new InputButton(keycode, XboxControllerButtons.A);
                    break;
                case "Attack":
                    Attack = new InputButton(keycode, XboxControllerButtons.RightBumper);
                    break;
                case "Paring":
                    Paring = new InputButton(keycode, XboxControllerButtons.Leftstick);
                    break;
                case "Interaction":
                    Interaction = new InputButton(keycode, XboxControllerButtons.X);
                    break;
                case "Dash":
                    Dash = new InputButton(keycode, XboxControllerButtons.LeftBumper);
                    break;
                case "Wave":
                    Wave = new InputButton(keycode, XboxControllerButtons.B);
                    break;
                case "Skill":
                    Skill = new InputButton(keycode, XboxControllerButtons.LeftBumper);
                    break;
                case "ItemT":
                    ItemT = new InputButton(keycode, XboxControllerButtons.Y);
                    break;
            }
        }
    }
    
}