using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Interaction : UI_WorldSpace
    {
        enum Images
        {
            Image_Background,
            Image_Interaction
        }

        float panelValue = 20f;
        float interactionValue = 20f;

        private Coroutine _panelCoroutine;
        private Coroutine _interactionCoroutine;
        public override void Init()
        {
            base.Init();
            Bind<Image>(typeof(Images));
            UIManager.Instance.KeyInputEvents -= HandleKeyInput;
            UIManager.Instance.KeyInputEvents += HandleKeyInput;
            Appear();
        }
        private void Appear()
        {
            _panelCoroutine = StartCoroutine(CoAppearPanel());
            _interactionCoroutine = StartCoroutine(CoAppearInteraction());
        }
        public void Disappear()
        {
            if (_panelCoroutine != null) StopCoroutine(_panelCoroutine);
            if (_interactionCoroutine != null) StopCoroutine(_interactionCoroutine);
            Get<Image>((int)Images.Image_Background).fillOrigin = (int)Image.OriginHorizontal.Right;
            UIManager.Instance.KeyInputEvents -= HandleKeyInput;
            StartCoroutine(CoDisAppearPanel());
            StartCoroutine(CoDisAppearInteraction());
        }
        private void HandleKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.N))
            {
                Disappear();
                StartCoroutine(CoCloseUI());
            }
        }
        IEnumerator CoAppearPanel()
        {
            float fill = Get<Image>((int)Images.Image_Background).fillAmount;
            while (true)
            {
                if (fill > 1)
                {
                    fill = 1f;
                    Get<Image>((int)Images.Image_Background).fillAmount = fill;
                    _panelCoroutine = null;
                    break;
                }
                fill += panelValue * Time.deltaTime;
                Get<Image>((int)Images.Image_Background).fillAmount = fill;
                yield return new WaitForSeconds(0.05f);
            }
        }
        IEnumerator CoAppearInteraction()
        {
            float alpha = Get<Image>((int)Images.Image_Interaction).color.a;
            while (true)
            {
                if (alpha > 1)
                {
                    alpha = 1f;
                    Get<Image>((int)Images.Image_Interaction).color = new Color(1, 1, 1, alpha);
                    _interactionCoroutine = null;
                    break;
                }
                alpha += interactionValue * Time.deltaTime;
                Get<Image>((int)Images.Image_Interaction).color = new Color(1, 1, 1, alpha);
                yield return new WaitForSeconds(0.05f);
            }
        }
        IEnumerator CoDisAppearPanel()
        {
            float fill = Get<Image>((int)Images.Image_Background).fillAmount;
            while (true)
            {
                if (fill < 0)
                {
                    fill = 0f;
                    Get<Image>((int)Images.Image_Background).fillAmount = fill;
                    _panelCoroutine = null;
                    break;
                }
                fill -= panelValue * Time.deltaTime;
                Get<Image>((int)Images.Image_Background).fillAmount = fill;
                yield return new WaitForSeconds(0.05f);
            }
        }
        IEnumerator CoDisAppearInteraction()
        {
            float alpha = Get<Image>((int)Images.Image_Interaction).color.a;
            while (true)
            {
                if (alpha < 0)
                {
                    alpha = 0f;
                    Get<Image>((int)Images.Image_Interaction).color = new Color(1, 1, 1, alpha);
                    _interactionCoroutine = null;
                    break;
                }
                alpha -= interactionValue * Time.deltaTime;
                Get<Image>((int)Images.Image_Interaction).color = new Color(1, 1, 1, alpha);
                yield return new WaitForSeconds(0.05f);
            }
        }
        IEnumerator CoCloseUI()
        {
            yield return new WaitForSeconds(0.5f);
            UIManager.Instance.CloseWorldSpaceUI(this);
        }
    }
}

