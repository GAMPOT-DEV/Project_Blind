using Blind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI_Base 클래스입니다. UI 스크립트들의 최상위 클래스 입니다.
/// </summary>


// UI_Base에서 UI_Scene과 UI_Popup으로 나눠집니다.
// UI_Scene : 각 씬마다 존재하는 고유한 UI (예) 메이플스토리의 HP UI)
// UI_Popup : 알림창처럼 팝업 형식으로 쌓이는 UI

namespace Blind
{
	public abstract class UI_Base : MonoBehaviour
	{
		protected int _uiNum;
		protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
		public abstract void Init();
		protected virtual void Start()
        {

        }
		protected virtual void Awake()
		{
			_uiNum = UIManager.Instance.UINum;
			Canvas canvas = GetComponent<Canvas>();
			if (canvas != null)
				canvas.sortingOrder = _uiNum;
			Init();
		}

		// UI를 자동으로 바인딩 해주는 함수
		// enum 값을 문자열로 변환할 수 있는 기능을 이용한다.
		// UI의 종류별로 _objects 딕셔너리에 저장된다.
		// UI_TestSceneUI 스크립트에 사용 예제가 있다.
		protected void Bind<T>(Type type) where T : UnityEngine.Object
		{
			string[] names = Enum.GetNames(type);
			UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
			_objects.Add(typeof(T), objects);

			for (int i = 0; i < names.Length; i++)
			{
				if (typeof(T) == typeof(GameObject))
					objects[i] = Util.FindChild(gameObject, names[i], true);
				else
					objects[i] = Util.FindChild<T>(gameObject, names[i], true);

				if (objects[i] == null)
					Debug.Log($"Failed to bind({names[i]})");
			}
		}

		// _objects에 저장된 UI를 쉽게 불러올 수 있는 함수
		// UI_TestSceneUI 스크립트에 사용 예제가 있다.
		protected T Get<T>(int idx) where T : UnityEngine.Object
		{
			UnityEngine.Object[] objects = null;
			if (_objects.TryGetValue(typeof(T), out objects) == false)
				return null;

			return objects[idx] as T;
		}

		// 자주 사용되는 UI들을 위해서 Get<T>를 매핑한 함수들
		protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
		protected Text GetText(int idx) { return Get<Text>(idx); }
		protected Button GetButton(int idx) { return Get<Button>(idx); }
		protected Image GetImage(int idx) { return Get<Image>(idx); }

		// UI에 콜백으로 이벤트를 할당하는 함수
		// UI_TestSceneUI 스크립트에 사용 예제가 있다.
		public static void BindEvent(GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
		{
			UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

			switch (type)
			{
				case Define.UIEvent.Click:
					evt.OnClickHandler -= action;
					evt.OnClickHandler += action;
					break;
				case Define.UIEvent.Drag:
					evt.OnDragHandler -= action;
					evt.OnDragHandler += action;
					break;
				case Define.UIEvent.EndDrag:
					evt.OnEndDragHandler -= action;
					evt.OnEndDragHandler += action;
					break;
				case Define.UIEvent.Enter:
					evt.OnEnterHandler -= action;
					evt.OnEnterHandler += action;
					break;
				case Define.UIEvent.Exit:
					evt.OnExitHandler -= action;
					evt.OnExitHandler += action;
					break;
			}
		}
	}

}
