using Blind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PoolManager Ŭ�����Դϴ�. ������Ʈ Ǯ���� �����մϴ�.
/// </summary>

namespace Blind
{
    public class PoolManager : Manager<PoolManager>
    {
        // ������Ʈ���� ����ִ� pool
        #region Pool
        class Pool
        {
            public GameObject Original { get; private set; }
            public Transform Root { get; set; }

            Stack<Poolable> _poolStack = new Stack<Poolable>();

            public void Init(GameObject original, int count = 5)
            {
                // Ǯ�� ����ִ� ������Ʈ�� ����
                Original = original;

                Root = new GameObject().transform;
                Root.name = $"{original.name}_Root";

                // count ��ŭ ������Ʈ�� �����ϰ� Ǯ�� �־��ش�.
                for (int i = 0; i < count; i++)
                    Push(Create());
            }

            Poolable Create()
            {
                GameObject go = Object.Instantiate<GameObject>(Original);
                go.name = Original.name;
                return go.GetOrAddComponent<Poolable>();
            }

            // Ǯ�� ������Ʈ�� �־��ִ� �Լ�.
            // Ǯ�� ������ �޸𸮿����� �����ϰ� ���ӿ����� ���̸� �ȵǱ� ������
            // ��Ȱ��ȭ ���·� �ٲ��ش�.
            public void Push(Poolable poolable)
            {
                if (poolable == null)
                    return;

                poolable.transform.parent = Root;
                poolable.gameObject.SetActive(false);
                poolable.IsUsing = false;

                _poolStack.Push(poolable);
            }

            // Ǯ���� ������Ʈ�� ������ �Լ�
            // Ǯ�� �ִ� ������Ʈ���� �̹� ��� ������̾ ���� �� ���ٸ�
            // ������Ʈ�� ���� ������ش�.
            // ���� ���� ������Ʈ�� ����� ������ Resource Manager���� Destory�� ȣ���ϸ�
            // Push�� ���� Ǯ�� ���� �ȴ�.
            public Poolable Pop(Transform parent)
            {
                Poolable poolable;

                if (_poolStack.Count > 0)
                    poolable = _poolStack.Pop();
                else
                    poolable = Create();

                poolable.gameObject.SetActive(true);

                // DontDestroyOnLoad ���� �뵵
                if (parent == null)
                    poolable.transform.parent = SceneController.Instance.CurrentScene.transform;

                poolable.transform.parent = parent;
                poolable.IsUsing = true;

                return poolable;
            }
        }
        #endregion

        // Pool ���� �����ϴ� ��ųʸ�
        Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
        Transform _root;

        protected override void Awake()
        {
            base.Awake();
            if (_root == null)
            {
                _root = new GameObject { name = "@Pool_Root" }.transform;
                Object.DontDestroyOnLoad(_root);
            }
        }

        // Ǯ�� ���� ������ִ� �Լ�
        public void CreatePool(GameObject original, int count = 5)
        {
            Pool pool = new Pool();
            pool.Init(original, count);
            pool.Root.parent = _root;

            _pool.Add(original.name, pool);
        }

        // ������Ʈ�� Ǯ�� Push ���ִ� �Լ�
        public void Push(Poolable poolable)
        {
            string name = poolable.gameObject.name;
            if (_pool.ContainsKey(name) == false)
            {
                GameObject.Destroy(poolable.gameObject);
                return;
            }

            _pool[name].Push(poolable);
        }

        // Ǯ���� pop�� �Ϸ� �ߴµ� Ǯ�� ���ٸ� Ǯ�� ���� ������ش�.
        // Ǯ���� ������Ʈ�� pop �ؼ� parent�� �ڽĿ� ���̰� ��ȯ���ش�. 
        public Poolable Pop(GameObject original, Transform parent = null)
        {
            if (_pool.ContainsKey(original.name) == false)
                CreatePool(original);

            return _pool[original.name].Pop(parent);
        }

        public GameObject GetOriginal(string name)
        {
            if (_pool.ContainsKey(name) == false)
                return null;
            return _pool[name].Original;
        }

        // root �ؿ� �ִ� ������Ʈ���� ���� �����Ѵ�.
        // ���� ����ǰų� �� �� ȣ���ϸ� �� ��?
        public void Clear()
        {
            foreach (Transform child in _root)
                GameObject.Destroy(child.gameObject);

            _pool.Clear();
        }
    }
}

