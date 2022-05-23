using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blind;

namespace Blind
{
    /// <summary>
    /// 상호작용이 가능한 오브젝트에 붙이려고 만든 클래스 (컴포넌트 형식으로 붙인다.)
    /// 지금은 임시로 만들어놔서 컴포넌트로 붙이면 같은 동작을 하지만
    /// 추상 클래스로 만들어서 상속받아서 유동적으로 사용할 수 있도록 수정할 예정
    /// </summary>
    
    // TODO : 추상 클래스로 수정해서 상속받아서 사용할수있도록
    public class InteractionAble : MonoBehaviour
    {
        BoxCollider2D _collider;
        UI_TestInteraction _ui;
        void Start()
        {
            _collider = gameObject.GetOrAddComponent<BoxCollider2D>();
            _collider.isTrigger = true;
            _collider.size = new Vector2(5, 5);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            // UI_TestInteraction를 WorldSpace로 띄운다.
            _ui = UIManager.Instance.ShowWorldSpaceUI<UI_TestInteraction>();
            _ui.SetPosition(gameObject.transform);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            _ui.CloseWorldSpaceUI();
        }
    }
}

