using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_LightControl : UI_Base
    {
        float speed = 5f;
        enum Images
        {
            Image_BeforeStart,
            Image_Line,

            Image_Dark,
            Image_Bright,
            Image_Guide1,

            Image_Guide2,
            Image_Button,
        }
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Get<Image>((int)Images.Image_Button).gameObject.BindEvent(PushButton);

            Get<Image>((int)Images.Image_BeforeStart).color = new Color(1, 1, 1, 0);
            Get<Image>((int)Images.Image_Line).color = new Color(1, 1, 1, 0);
            Get<Image>((int)Images.Image_Dark).color = new Color(1, 1, 1, 0);
            Get<Image>((int)Images.Image_Bright).color = new Color(1, 1, 1, 0);
            Get<Image>((int)Images.Image_Guide1).color = new Color(1, 1, 1, 0);
            Get<Image>((int)Images.Image_Guide2).color = new Color(1, 1, 1, 0);
            Get<Image>((int)Images.Image_Button).color = new Color(1, 1, 1, 0);

            StartCoroutine(CoApearImage1());
        }
        IEnumerator CoApearImage1()
        {
            float alpha = 0;
            while (true)
            {
                alpha += speed;
                if (alpha >= 255)
                {
                    alpha = 255f;
                    Get<Image>((int)Images.Image_BeforeStart).color = new Color(1, 1, 1, alpha / 255f);
                    Get<Image>((int)Images.Image_Line).color = new Color(1, 1, 1, alpha / 255f);
                    StartCoroutine(CoApearImage2());
                    break;
                }
                Get<Image>((int)Images.Image_BeforeStart).color = new Color(1, 1, 1, alpha / 255f);
                Get<Image>((int)Images.Image_Line).color = new Color(1, 1, 1, alpha / 255f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        IEnumerator CoApearImage2()
        {
            float alpha = 0;
            while (true)
            {
                alpha += speed;
                if (alpha >= 255)
                {
                    alpha = 255f;
                    Get<Image>((int)Images.Image_Dark).color = new Color(1, 1, 1, alpha / 255f);
                    Get<Image>((int)Images.Image_Bright).color = new Color(1, 1, 1, alpha / 255f);
                    Get<Image>((int)Images.Image_Guide1).color = new Color(1, 1, 1, alpha / 255f);
                    StartCoroutine(CoApearImage3());
                    break;
                }
                Get<Image>((int)Images.Image_Dark).color = new Color(1, 1, 1, alpha / 255f);
                Get<Image>((int)Images.Image_Bright).color = new Color(1, 1, 1, alpha / 255f);
                Get<Image>((int)Images.Image_Guide1).color = new Color(1, 1, 1, alpha / 255f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        IEnumerator CoApearImage3()
        {
            float alpha = 0;
            while (true)
            {
                alpha += speed;
                if (alpha >= 255)
                {
                    alpha = 255f;
                    Get<Image>((int)Images.Image_Guide2).color = new Color(1, 1, 1, alpha / 255f);
                    Get<Image>((int)Images.Image_Button).color = new Color(1, 1, 1, alpha / 255f);
                    break;
                }
                Get<Image>((int)Images.Image_Guide2).color = new Color(1, 1, 1, alpha / 255f);
                Get<Image>((int)Images.Image_Button).color = new Color(1, 1, 1, alpha / 255f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        private void PushButton()
        {
            UIManager.Instance.Clear();
            Debug.Log("Push Button");
        }
    }
}

