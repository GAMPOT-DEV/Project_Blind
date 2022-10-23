using System.Collections;
using UnityEngine;

namespace Blind
{
    public class ObjectFadeManager : MonoBehaviour
    {
        GameObject objects;
        private SpriteRenderer _sprite;
        
        public static ObjectFadeManager Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;
                s_Instance = FindObjectOfType<ObjectFadeManager>();

                if (s_Instance != null)
                    return s_Instance;

                return s_Instance;

            }
        }
        
        public static ObjectFadeManager s_Instance;
        public void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);

            _sprite = objects.GetComponent<SpriteRenderer>();
        }

        public IEnumerator FadeIn()
        {
            for (int i = 0; i < 10; i++)
            {
                float f = i / 10.0f;
                Color c = _sprite.material.color;
                c.a = f;
                _sprite.material.color = c;
                yield return new WaitForSeconds(0.1f);
            }
        }

        public IEnumerator FadeOut()
        {
            Debug.Log("싫애도;ㅁ");
            for (int i = 10; i >= 0; i--)
            {
                float f = i / 10.0f;
                Color c = _sprite.material.color;
                c.a = f;
                _sprite.material.color = c;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}