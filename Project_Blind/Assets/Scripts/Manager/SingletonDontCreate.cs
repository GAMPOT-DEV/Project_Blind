using UnityEngine;

namespace Blind
{
    public abstract class SingletonDontCreate<T> : MonoBehaviour,ISingleton
        where T : MonoBehaviour, ISingleton
    {
        private static T s_instance = null;

        public static T Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;
                Find();
                return s_instance;
            }
        }
        protected virtual void Awake()
        {
            if(s_instance == null)
                Find();
        }


        private static void Find()
        {
            s_instance = FindObjectOfType<T>(); // 현재 씬에 클래스가 있는지 확인
        }
    }
}