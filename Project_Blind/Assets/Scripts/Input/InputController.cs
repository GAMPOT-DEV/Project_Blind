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

        public Dictionary<Define.KeyAction, KeyCode> CurrKeyActions;

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
        public InputButton DownJump;
        public InputButton LeftMove;
        public InputButton RightMove;
        public InputButton ChangeSlot;


        public InputAxis Horizontal;
        public InputAxis Vertical;
        // </키들을 정의하는 공간입니다>
        
        protected override void Awake()
        {
            base.Awake();
            // 조작키를 할당합니다.
            Jump = new InputButton(KeyCode.W,XboxControllerButtons.A);
            DownJump = new InputButton(KeyCode.S, XboxControllerButtons.B);
            LeftMove = new InputButton(KeyCode.A, XboxControllerButtons.A);
            RightMove = new InputButton(KeyCode.D, XboxControllerButtons.X);
            Paring = new InputButton(KeyCode.K, XboxControllerButtons.Leftstick);
            Interaction = new InputButton(KeyCode.N, XboxControllerButtons.X);
            Attack = new InputButton(KeyCode.J, XboxControllerButtons.RightBumper);
            Dash = new InputButton(KeyCode.Space, XboxControllerButtons.LeftBumper);
            Wave = new InputButton(KeyCode.LeftControl,XboxControllerButtons.B);
            Horizontal = new InputAxis(KeyCode.D, KeyCode.A, XboxControllerAxes.LeftstickHorizontal);
            Vertical = new InputAxis(KeyCode.W,KeyCode.S,XboxControllerAxes.LeftstickVertical);
            Skill = new InputButton(KeyCode.I, XboxControllerButtons.LeftBumper);
            ChangeSlot = new InputButton(KeyCode.E, XboxControllerButtons.X);
            ItemT = new InputButton(KeyCode.Q, XboxControllerButtons.Y);

            InitialKeyActions.Add(Define.KeyAction.Jump, KeyCode.W);
            InitialKeyActions.Add(Define.KeyAction.DownJump, KeyCode.S);
            InitialKeyActions.Add(Define.KeyAction.LeftMove, KeyCode.A);
            InitialKeyActions.Add(Define.KeyAction.RightMove, KeyCode.D);
            InitialKeyActions.Add(Define.KeyAction.Paring, KeyCode.K);
            InitialKeyActions.Add(Define.KeyAction.Interaction, KeyCode.N);
            InitialKeyActions.Add(Define.KeyAction.Attack, KeyCode.J);
            InitialKeyActions.Add(Define.KeyAction.Dash, KeyCode.Space);
            InitialKeyActions.Add(Define.KeyAction.Wave, KeyCode.LeftControl);
            InitialKeyActions.Add(Define.KeyAction.Skill, KeyCode.I);
            InitialKeyActions.Add(Define.KeyAction.ChangeSlot, KeyCode.E);
            InitialKeyActions.Add(Define.KeyAction.ItemT, KeyCode.Q);

            CurrKeyActions = new Dictionary<Define.KeyAction, KeyCode>(InitialKeyActions);
        }

        /// <summary>
        /// 입력을 가져오는 함수입니다.
        /// </summary>
        /// <param name="fixedUpdateHappened"></param>
        protected override void GetInputs(bool fixedUpdateHappened)
        {
            _HaveControl = true;
            
            Jump.Get(fixedUpdateHappened,inputType);
            DownJump.Get(fixedUpdateHappened, inputType);
            LeftMove.Get(fixedUpdateHappened, inputType);
            RightMove.Get(fixedUpdateHappened, inputType);
            Paring.Get(fixedUpdateHappened, inputType);
            Interaction.Get(fixedUpdateHappened, inputType);
            Dash.Get(fixedUpdateHappened, inputType);
            Attack.Get(fixedUpdateHappened, inputType);
            Wave.Get(fixedUpdateHappened,inputType);
            Horizontal.Get(inputType);
            Vertical.Get(inputType);
            Skill.Get(fixedUpdateHappened, inputType);
            ItemT.Get(fixedUpdateHappened, inputType);
            ChangeSlot.Get(fixedUpdateHappened, inputType);
        }
        /// <summary>
        /// 막은 키 입력을 다시 활성화 합니다.
        /// </summary>
        /// <param name="resetValues"></param>

        public override void GainControl()
        {
            _HaveControl = true;
            
            GainControl(Jump);
            GainControl(DownJump);
            GainControl(LeftMove);
            GainControl(RightMove);
            GainControl(Paring);
            GainControl(Interaction);
            GainControl(Horizontal);
            GainControl(Vertical);
            GainControl(Dash);
            GainControl(Wave);
            GainControl(Attack);
            GainControl(Skill);
            GainControl(ItemT);
            GainControl(ChangeSlot);
        }
        
        /// <summary>
        /// 플레이어의 키 입력을 막습니다.
        /// </summary>
        /// <param name="resetValues"></param>

        public override void ReleaseControl(bool resetValues = true)
        {
            _HaveControl = false;
            
            ReleaseControl(Jump,resetValues);
            ReleaseControl(DownJump, resetValues);
            ReleaseControl(LeftMove, resetValues);
            ReleaseControl(RightMove, resetValues);
            ReleaseControl(Paring, resetValues);
            ReleaseControl(Interaction, resetValues);
            ReleaseControl(Horizontal,resetValues);
            ReleaseControl(Vertical,resetValues);
            ReleaseControl(Dash,resetValues);
            ReleaseControl(Wave,resetValues);
            ReleaseControl(Attack, resetValues);
            ReleaseControl(Skill, resetValues);
            ReleaseControl(ItemT, resetValues);
            ReleaseControl(ChangeSlot, resetValues);
        }

        public void ReKetSet()
        {
            foreach (Define.KeyAction action in CurrKeyActions.Keys)
            {
                KeyCode key = CurrKeyActions[action];
                switch (action)
                {
                    case Define.KeyAction.Jump:
                        // 이렇게?
                        Jump = new InputButton(key, XboxControllerButtons.A);
                        break;
                    case Define.KeyAction.DownJump:
                        break;
                    case Define.KeyAction.LeftMove:
                        break;
                    case Define.KeyAction.RightMove:
                        break;
                    case Define.KeyAction.Paring:
                        break;
                    case Define.KeyAction.Interaction:
                        break;
                    case Define.KeyAction.Attack:
                        break;
                    case Define.KeyAction.Dash:
                        break;
                    case Define.KeyAction.Wave:
                        break;
                    case Define.KeyAction.Skill:
                        break;
                    case Define.KeyAction.ChangeSlot:
                        break;
                    case Define.KeyAction.ItemT:
                        break;
                }
            }
        }
    }
    
}