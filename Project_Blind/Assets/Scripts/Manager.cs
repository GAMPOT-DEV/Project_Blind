using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 매니저들의 부모 클래스입니다. 싱글톤이 정의되어 있습니다.
    /// </summary>
    public abstract class Manager<T> : MonoBehaviour ,IManager
        where T : MonoBehaviour, IManager
    {
        private static IManager s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance as T;
                Create();
                return s_instance as T;
            }
        }
        protected virtual void Awake()
        {
            if(s_instance == null)
                Create();
        }

        private static void Create()
        {
            var className = typeof(T).Name; // 클래스 이름 가져오기
            var managerGameObject = new GameObject(className); // 게임 오브젝트 새로 만들기
            managerGameObject.AddComponent<T>(); // 클래스 붙이기
        }
        
    }
}