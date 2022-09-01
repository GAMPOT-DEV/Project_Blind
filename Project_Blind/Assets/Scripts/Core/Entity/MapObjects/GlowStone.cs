using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Blind
{
    [RequireComponent(typeof(Light2D))]
    public class GlowStone : InteractionAble
    {
        [SerializeField] private AnimationCurve spreadAnimation;
        [SerializeField] private float maxSize = 30f;
        [SerializeField] private float time;

        private Light2D _light2D;
        private bool _isBright = false;
        [SerializeField] private bool _isReady = false;

        protected override void Awake()
        {
            base.Awake();
            _light2D = GetComponent<Light2D>();
        }
        
        /// <summary>
        /// 이 함수 호출하면 켜집니다.
        /// </summary>
        public void Bright()
        {
            if (_isBright) return;
            StartCoroutine(_Bright());
            _isBright = true;
        }

        public IEnumerator _Bright()
        {
            var curTime = 0f;
            var radius = 0f;
            while (radius < maxSize)
            {
                curTime += Time.deltaTime;
                radius += spreadAnimation.Evaluate(curTime / time) * maxSize;
                _light2D.pointLightOuterRadius = radius;
                yield return null;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null)
                return;

            UIManager.Instance.KeyInputEvents -= HandleKeyInput;
            UIManager.Instance.KeyInputEvents += HandleKeyInput;

            _ui = UIManager.Instance.ShowWorldSpaceUI<UI_TestInteraction>();
            _ui.SetPosition(transform.position, Vector3.down * 3);
            _isReady = true;
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null)
                return;

            UIManager.Instance.KeyInputEvents -= HandleKeyInput;

            if (_ui != null)
                _ui.CloseWorldSpaceUI();
            _isReady = false;
        }

        public override void DoInteraction()
        {
            Bright();
            ActivateInvisibleFloor();
        }

        private void HandleKeyInput()
        {
            if (InputController.Instance.Interaction.Down)
            {
                if (_isReady)
                {
                    if (_ui != null)
                        _ui.CloseWorldSpaceUI();
                    DoInteraction();

                    UIManager.Instance.KeyInputEvents -= HandleKeyInput;
                }
            }
        }

        private void ActivateInvisibleFloor()
        {
            GameObject[] floors = GameObject.FindGameObjectsWithTag("InvisibleFloor");
            for (int i = 0; i < floors.Length; i++)
            {
                floors[i].GetComponent<InvisibleFloor>().SetVisible();
            }
        }
    }
}