using Blind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PoolManager Ŭ�����Դϴ�. UI�� �����մϴ�.
/// </summary>

namespace Blind
{
    public class UIManager : Manager<UIManager>
    {
        // sortingOrder�� �����ϱ� ���� ����
        int _order = 10;

        // �˾� UI���� ����ִ� ����
        Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

        // �� ������ �ִ� ������ UI
        public UI_Scene SceneUI { get; private set; }

        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find("@UI_Root");
                if (root == null)
                    root = new GameObject { name = "@UI_Root" };
                return root;
            }
        }

        public void SetCanvas(GameObject go, bool sort = true)
        {
            Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;

            if (sort)
            {
                canvas.sortingOrder = _order;
                _order++;
            }
            else
            {
                canvas.sortingOrder = 0;
            }
        }
        // ���� �ִ� ������ UI�� �����ִ� �Լ�.
        // ���� ���۵� �� �ѹ� ȣ���ϸ� �ɵ�?
        public T ShowSceneUI<T>(string name = null) where T : UI_Scene
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = ResourceManager.Instance.Instantiate($"UI/Scene/{name}");
            T sceneUI = Util.GetOrAddComponent<T>(go);
            SceneUI = sceneUI;

            go.transform.SetParent(Root.transform);

            return sceneUI;
        }

        // �˾� UI�� ����ִ� �Լ�
        public T ShowPopupUI<T>(string name = null) where T : UI_Popup
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = ResourceManager.Instance.Instantiate($"UI/Popup/{name}");
            T popup = Util.GetOrAddComponent<T>(go);
            _popupStack.Push(popup);

            go.transform.SetParent(Root.transform);

            return popup;
        }
        // �˾� UI�� �ݾ��ִ� �Լ�
        public void ClosePopupUI(UI_Popup popup)
        {
            if (_popupStack.Count == 0)
                return;

            if (_popupStack.Peek() != popup)
            {
                Debug.Log("Close Popup Failed!");
                return;
            }

            ClosePopupUI();
        }

        public void ClosePopupUI()
        {
            if (_popupStack.Count == 0)
                return;

            UI_Popup popup = _popupStack.Pop();
            ResourceManager.Instance.Destroy(popup.gameObject);
            popup = null;
            _order--;
        }
        // ��� �˾� UI�� �ݾ��ִ� �Լ�
        public void CloseAllPopupUI()
        {
            while (_popupStack.Count > 0)
                ClosePopupUI();
        }

        public void Clear()
        {
            CloseAllPopupUI();
            SceneUI = null;
        }
    }
}

