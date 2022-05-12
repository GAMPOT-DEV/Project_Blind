using Blind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ResourceManager Ŭ�����Դϴ�. ���ҽ����� �����մϴ�.
/// </summary>

namespace Blind
{
    public class ResourceManager : Manager<ResourceManager>
    {
        // PoolManager�� ����Ǿ� ������ �װ��� ��������,
        // �ƴϸ� ���� Load �Ѵ�.
        public T Load<T>(string path) where T : Object
        {
            if (typeof(T) == typeof(GameObject))
            {
                string name = path;

                // path���� ������Ʈ�� �̸��� ����
                int index = name.LastIndexOf('/');
                if (index >= 0)
                    name = name.Substring(index + 1);

                GameObject go = PoolManager.Instance.GetOriginal(name);
                if (go != null)
                    return go as T;
            }

            return Resources.Load<T>(path);
        }

        // Poolable ������Ʈ�� �پ��ִٸ� ������Ʈ Ǯ���� �ϰ�,
        // �ƴϸ� �׳� �����Ѵ�.
        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject original = Load<GameObject>($"Prefabs/{path}");
            if (original == null)
            {
                Debug.Log($"Failed to load prefab : {path}");
                return null;
            }

            // Poolable ������Ʈ�� �پ��ִٸ� ������Ʈ Ǯ���� �Ѵ�.
            if (original.GetComponent<Poolable>() != null)
                return PoolManager.Instance.Pop(original, parent).gameObject;

            GameObject go = Object.Instantiate(original, parent);
            go.name = original.name;
            return go;
        }

        // ������Ʈ Ǯ�� ���� ������Ʈ��� �ٽ� Ǯ�� �ְ�, �ƴ϶�� Destroy�� �Ѵ�.
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
