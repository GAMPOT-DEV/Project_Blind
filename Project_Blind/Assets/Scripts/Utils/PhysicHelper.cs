using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Blind
{
    /// <summary>
    /// 현재는 레이캐스트가 맞은 오브젝트에 PlatformEffector가 있는지 판별하는 클래스(나중에 플랫폼 이펙트 말고도 다른 걸 판별할 때 써도됨)
    /// </summary>
    public class PhysicHelper : MonoBehaviour
    {
        private static PhysicHelper s_Instance;

        static PhysicHelper Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;

                s_Instance = FindObjectOfType<PhysicHelper> ();

                if (s_Instance != null)
                    return s_Instance;
            
                Create ();
            
                return s_Instance;
            }
            set
            {
                s_Instance = value;
            }
        }

        static void Create()
        {
            GameObject physicsHelperGameObject = new GameObject("PhysicsHelper");
            s_Instance = physicsHelperGameObject.AddComponent<PhysicHelper> ();
        }
        Dictionary<Collider2D, PlatformEffector2D> m_PlatformEffectorCache = new Dictionary<Collider2D, PlatformEffector2D> ();

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy (gameObject);
                return;
            }

            PopulateColliderDictionary(m_PlatformEffectorCache);
        }

        protected void PopulateColliderDictionary<TComponent>(Dictionary<Collider2D, TComponent> dict)
            where TComponent : Component
        {
            TComponent[] components = FindObjectsOfType<TComponent>();

            for (int i = 0; i < components.Length; i++)
            {
                Collider2D[] componentColliders = components[i].GetComponents<Collider2D>();

                for (int j = 0; j < componentColliders.Length; j++)
                {
                    dict.Add(componentColliders[j], components[i]);
                }
            }
        }

        public static bool ColliderHasPlatformEffector (Collider2D collider)
        {
            return Instance.m_PlatformEffectorCache.ContainsKey (collider);
        }
        
        public static bool TryGetPlatformEffector (Collider2D collider, out PlatformEffector2D platformEffector)
        {
            return Instance.m_PlatformEffectorCache.TryGetValue (collider, out platformEffector);
        }
    }
}