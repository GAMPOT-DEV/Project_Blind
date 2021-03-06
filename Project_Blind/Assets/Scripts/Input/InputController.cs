using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 입력을 받는 매니저입니다.
    /// 키 입력을 할당하고 어떤 키가 어떤 상태인지 확인할 수 있습니다.
    /// </summary>
    public class InputController : InputComponent<InputController>
    {
        protected bool _HaveControl = true;

        protected bool _DebugMenuIsOpen = false;
        // <키들을 정의하는 공간입니다>
        public InputButton Jump;
        public InputAxis Horizontal;
        public InputAxis Vertical;
        // </키들을 정의하는 공간입니다>
        
        protected override void Awake()
        {
            base.Awake();
            // 조작키를 할당합니다.
            Jump = new InputButton(KeyCode.Space,XboxControllerButtons.A);
            Horizontal = new InputAxis(KeyCode.D,KeyCode.A,XboxControllerAxes.LeftstickHorizontal);
            Vertical = new InputAxis(KeyCode.W,KeyCode.S,XboxControllerAxes.LeftstickVertical);
        }

        /// <summary>
        /// 입력을 가져오는 함수입니다.
        /// </summary>
        /// <param name="fixedUpdateHappened"></param>
        protected override void GetInputs(bool fixedUpdateHappened)
        {
            _HaveControl = true;
            
            Jump.Get(fixedUpdateHappened,inputType);
            Horizontal.Get(inputType);
            Vertical.Get(inputType);
        }
        /// <summary>
        /// 막은 키 입력을 다시 활성화 합니다.
        /// </summary>
        /// <param name="resetValues"></param>

        public override void GainControl()
        {
            _HaveControl = true;
            
            GainControl(Jump);
            GainControl(Horizontal);
            GainControl(Vertical);
        }
        
        /// <summary>
        /// 플레이어의 키 입력을 막습니다.
        /// </summary>
        /// <param name="resetValues"></param>

        public override void ReleaseControl(bool resetValues = true)
        {
            _HaveControl = false;
            
            ReleaseControl(Jump,resetValues);
            ReleaseControl(Horizontal,resetValues);
            ReleaseControl(Vertical,resetValues);
        }
    }
}