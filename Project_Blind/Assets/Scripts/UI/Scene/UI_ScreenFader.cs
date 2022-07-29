using System;
using System.Collections;
using UnityEngine;

namespace Blind
{
    public class UI_ScreenFader : MonoBehaviour
    {
        public static UI_ScreenFader Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;
                s_Instance = FindObjectOfType<UI_ScreenFader>();

                if (s_Instance != null)
                    return s_Instance;

                return s_Instance;

            }
        }

        protected static UI_ScreenFader s_Instance;

        public static void Create()
        {
            UI_ScreenFader controllerPrefab =  ResourceManager.Instance.Load<UI_ScreenFader>("Resource/Prefab/ScreenFader");
            s_Instance = Instantiate(controllerPrefab);
        }
        
        public CanvasGroup FaderCanvasGroup;
        public float fadeDuration = 1f;

        protected bool isFading;

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance.FaderCanvasGroup.alpha = 1f;
            
            DontDestroyOnLoad(gameObject);
        }

        protected IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
        {
            isFading = true;
            canvasGroup.blocksRaycasts = true;
            float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;
            while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha,
                    fadeSpeed * Time.deltaTime);
                yield return null;
            }

            canvasGroup.alpha = finalAlpha;
            isFading = false;
            canvasGroup.blocksRaycasts = false;
        }
        public static IEnumerator FadeSceneIn ()
        {
            CanvasGroup canvasGroup;
            canvasGroup = Instance.FaderCanvasGroup;

            yield return Instance.StartCoroutine(Instance.Fade(0f, canvasGroup));

            canvasGroup.gameObject.SetActive(false);
        }

        public static IEnumerator FadeScenOut()
        {
            CanvasGroup canvasGroup = Instance.FaderCanvasGroup;
            canvasGroup.gameObject.SetActive(true);
            yield return Instance.StartCoroutine(Instance.Fade(1f, canvasGroup));
        }
    }
}