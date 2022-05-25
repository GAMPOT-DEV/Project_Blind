using Blind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PoolManager 클래스입니다. 오브젝트 풀링을 관리합니다.
/// </summary>

namespace Blind
{
    public class PoolManager : Manager<PoolManager>
    {
        // 오브젝트들이 담겨있는 pool
        #region Pool
        class Pool
        {
            public GameObject Original { get; private set; }
            public Transform Root { get; set; }

            Stack<Poolable> _poolStack = new Stack<Poolable>();

            public void Init(GameObject original, int count = 5)
            {
                // 풀이 담고있는 오브젝트의 원본
                Original = original;

                Root = new GameObject().transform;
                Root.name = $"{original.name}_Root";

                // count 만큼 오브젝트를 생성하고 풀에 넣어준다.
                for (int i = 0; i < count; i++)
                    Push(Create());
            }

            Poolable Create()
            {
                GameObject go = Object.Instantiate<GameObject>(Original);
                go.name = Original.name;
                return go.GetOrAddComponent<Poolable>();
            }

            // 풀에 오브젝트를 넣어주는 함수.
            // 풀에 있으면 메모리에서만 존재하고 게임에서는 보이면 안되기 때문에
            // 비활성화 상태로 바꿔준다.
            public void Push(Poolable poolable)
            {
                if (poolable == null)
                    return;

                poolable.transform.SetParent(Root);
                poolable.gameObject.SetActive(false);
                poolable.IsUsing = false;

                _poolStack.Push(poolable);
            }

            // 풀에서 오브젝트를 꺼내는 함수
            // 풀에 있는 오브젝트들이 이미 모두 사용중이어서 꺼낼 수 없다면
            // 오브젝트를 새로 만들어준다.
            // 새로 만든 오브젝트도 사용이 끝나고 Resource Manager에서 Destory를 호출하면
            // Push를 통해 풀에 들어가게 된다.
            public Poolable Pop(Transform parent)
            {
                Poolable poolable;

                if (_poolStack.Count > 0)
                    poolable = _poolStack.Pop();
                else
                    poolable = Create();

                poolable.gameObject.SetActive(true);

                // DontDestroyOnLoad 해제 용도
                if (parent == null)
                    poolable.transform.SetParent(ResourceManager.Instance.PrefabsRoot.transform);
                else
                    poolable.transform.parent = parent;

                poolable.IsUsing = true;

                return poolable;
            }
        }
        #endregion

        // Pool 들을 관리하는 딕셔너리
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

        // 풀을 새로 만들어주는 함수
        public void CreatePool(GameObject original, int count = 5)
        {
            Pool pool = new Pool();
            pool.Init(original, count);
            pool.Root.parent = _root;

            _pool.Add(original.name, pool);
        }

        // 오브젝트를 풀에 Push 해주는 함수
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

        // 풀에서 pop을 하려 했는데 풀이 없다면 풀을 새로 만들어준다.
        // 풀에서 오브젝트를 pop 해서 parent의 자식에 붙이고 반환해준다. 
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

        // root 밑에 있는 오브젝트들을 전부 삭제한다.
        // 씬이 변경되거나 할 때 호출하면 될 듯?
        public void Clear()
        {
            foreach (Transform child in _root)
                GameObject.Destroy(child.gameObject);

            _pool.Clear();
        }
    }
}

