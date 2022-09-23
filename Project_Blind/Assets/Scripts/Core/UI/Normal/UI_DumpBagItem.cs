using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_DumpBagItem : UI_Base
    {
        private UI_Bag _bag;
        enum Texts
        {
            Text_Input,
        }
        enum Buttons
        {
            Button_Submit,
        }
        public override void Init()
        {
            Bind<Text>(typeof(Texts));
            Bind<Button>(typeof(Buttons));

            Get<Button>((int)Buttons.Button_Submit).gameObject.BindEvent(PushSubmitButton);
        }
        public void AdditionalInit(UI_Bag bag)
        {
            _bag = bag;
        }
        
        private void PushSubmitButton()
        {
            string text = Get<Text>((int)Texts.Text_Input).text;
            try
            {
                int result = Int32.Parse(text);
                if (result > 10000)
                {
                    UIManager.Instance.CloseNormalUI(this);
                    return;
                }
                _bag.DumpItem(result);
                UIManager.Instance.CloseNormalUI(this);
            }
            catch (FormatException)
            {
                Debug.Log("Error");
                UIManager.Instance.CloseNormalUI(this);
            }
        }
    }
}

