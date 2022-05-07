using Blind;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI_Base Ŭ�����Դϴ�. UI ��ũ��Ʈ���� �ֻ��� Ŭ���� �Դϴ�.
/// </summary>


// UI_Base���� UI_Scene�� UI_Popup���� �������ϴ�.
// UI_Scene : �� ������ �����ϴ� ������ UI (��) �����ý��丮�� HP UI)
// UI_Popup : �˸�âó�� �˾� �������� ���̴� UI

namespace Blind
{
	public abstract class UI_Base : MonoBehaviour
	{
		protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
		public abstract void Init();

		private void Awake()
		{
			Init();
		}

		// UI�� �ڵ����� ���ε� ���ִ� �Լ�
		// enum ���� ���ڿ��� ��ȯ�� �� �ִ� ����� �̿��Ѵ�.
		// UI�� �������� _objects ��ųʸ��� ����ȴ�.
		// UI_TestSceneUI ��ũ��Ʈ�� ��� ������ �ִ�.
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

		// _objects�� ����� UI�� ���� �ҷ��� �� �ִ� �Լ�
		// UI_TestSceneUI ��ũ��Ʈ�� ��� ������ �ִ�.
		protected T Get<T>(int idx) where T : UnityEngine.Object
		{
			UnityEngine.Object[] objects = null;
			if (_objects.TryGetValue(typeof(T), out objects) == false)
				return null;

			return objects[idx] as T;
		}

		// ���� ���Ǵ� UI���� ���ؼ� Get<T>�� ������ �Լ���
		protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
		protected Text GetText(int idx) { return Get<Text>(idx); }
		protected Button GetButton(int idx) { return Get<Button>(idx); }
		protected Image GetImage(int idx) { return Get<Image>(idx); }

		// UI�� �ݹ����� �̺�Ʈ�� �Ҵ��ϴ� �Լ�
		// UI_TestSceneUI ��ũ��Ʈ�� ��� ������ �ִ�.
		public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
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
			}
		}
	}

}
