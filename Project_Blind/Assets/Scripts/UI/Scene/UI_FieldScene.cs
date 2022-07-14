using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class UI_FieldScene : UI_Scene
    {
        public override void Init()
        {
            base.Init();
        }
        private void Update()
        {
            HandleUIKeyInput();
        }
        private void HandleUIKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
            {
                //Debug.Log(_uiNum);
                //Debug.Log(UIManager.Instance.UINum);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // TODO 메뉴 UI
                Debug.Log("ESC");
                UIManager.Instance.ShowNormalUI<UI_Menu>();
            }
        }
    }
}

