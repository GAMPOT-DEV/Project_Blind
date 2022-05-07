using Blind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ResourceManager 클래스입니다. 리소스들을 관리합니다.
/// </summary>

namespace Blind
{
    public class ResourceManager : Manager<ResourceManager>
    {
        // PoolManager에 저장되어 있으면 그것을 가져오고,
        // 아니면 새로 Load 한다.
        public T Load<T>(string path) where T : Object
        {
            if (typeof(T) == typeof(GameObject))
            {
                string name = path;

                // path에서 오브젝트의 이름을 추출
                int index = name.LastIndexOf('/');
                if (index >= 0)
                    name = name.Substring(index + 1);

                GameObject go = PoolManager.Instance.GetOriginal(name);
                if (go != null)
                    return go as T;
            }

            return Resources.Load<T>(path);
        }

        // Poolable 컴포넌트가 붙어있다면 오브젝트 풀링을 하고,
        // 아니면 그냥 생성한다.
        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject original = Load<GameObject>($"Prefabs/{path}");
            if (original == null)
            {
                Debug.Log($"Failed to load prefab : {path}");
                return null;
            }

            // Poolable 컴포넌트가 붙어있다면 오브젝트 풀링을 한다.
            if (original.GetComponent<Poolable>() != null)
                return PoolManager.Instance.Pop(original, parent).gameObject;

            GameObject go = Object.Instantiate(original, parent);
            go.name = original.name;
            return go;
        }

        // 오브젝트 풀링 중인 오브젝트라면 다시 풀에 넣고, 아니라면 Destroy를 한다.
        public void Destroy(GameObject go)
        {
            if (go == null)
                return;

            Poolable poolable = go.GetComponent<Poolable>();
            if (poolable != null)
            {
                PoolManager.Instance.Push(poolable);
                return;
            }

            Object.Destroy(go);
        }
    }

}
