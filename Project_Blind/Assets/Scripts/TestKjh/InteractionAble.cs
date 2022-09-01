using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blind;

namespace Blind
{
    /// <summary>
    /// 상호작용이 가능한 오브젝트에 붙이려고 만든 클래스 (컴포넌트 형식으로 붙인다.)
    /// </summary>
    
    public abstract class InteractionAble : MonoBehaviour
    {
        protected enum State
        {
            NonKeyDown,
            KeyDown,
            Ing
        }

        protected BoxCollider2D _collider;
        protected UI_WorldSpace _ui;
        protected virtual void Init(int x = 5, int y = 5)
        {
            _collider = gameObject.GetOrAddComponent<BoxCollider2D>();
            _collider.isTrigger = true;
            _collider.size = new Vector2(x, y);
        }
        protected virtual void Awake()
        {
            Init();
        }
        // 상호작용 범위 안에 들어왔을 때
        protected abstract void OnTriggerEnter2D(Collider2D collision);
        // 상호작용 범위에서 벗어났을 때
        protected abstract void OnTriggerExit2D(Collider2D collision);
        // 상호작용 키를 눌렀을때 호출
        public abstract void DoInteraction();

    }
}

