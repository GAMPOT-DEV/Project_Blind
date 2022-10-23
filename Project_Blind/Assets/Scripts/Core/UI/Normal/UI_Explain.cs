using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Explain : UI_Base
    {
        enum Images
        {
            Image_Explain
        }
        enum Buttons
        {
            Button_RightArrow,
            Button_LeftArrow,
            Button_Close
        }
        [SerializeField] Sprite[] Sprites;
        int _currIndex = 0;
        public override void Init()
        {
            Time.timeScale = 0;

            Bind<Image>(typeof(Images));
            Bind<Button>(typeof(Buttons));
            Get<Button>((int)Buttons.Button_RightArrow).gameObject.BindEvent(() => PushArrowButton(1));
            Get<Button>((int)Buttons.Button_LeftArrow).gameObject.BindEvent(() => PushArrowButton(-1));
            Get<Button>((int)Buttons.Button_Close).gameObject.BindEvent(PushCloseButton);

            Get<Image>((int)Images.Image_Explain).sprite = Sprites[_currIndex];
            RefreshArrow();
        }
        private void RefreshArrow()
        {
            Get<Button>((int)Buttons.Button_RightArrow).gameObject.SetActive(true);
            Get<Button>((int)Buttons.Button_LeftArrow).gameObject.SetActive(true);
            if (_currIndex == 0) Get<Button>((int)Buttons.Button_LeftArrow).gameObject.SetActive(false);
            if (_currIndex == Sprites.Length - 1) Get<Button>((int)Buttons.Button_RightArrow).gameObject.SetActive(false);
        }
        private void PushArrowButton(int dir)
        {
            int nextIndex = _currIndex + dir;
            if (nextIndex < 0 || nextIndex > Sprites.Length - 1) return;
            _currIndex = nextIndex;

            Get<Image>((int)Images.Image_Explain).sprite = Sprites[_currIndex];
            RefreshArrow();
        }
        private void PushCloseButton()
        {
            Time.timeScale = 1;
            Destroy(this.gameObject);
        }
    }
}

