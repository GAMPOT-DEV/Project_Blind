using Blind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIManager 클래스입니다. UI를 관리합니다.
/// </summary>

namespace Blind
{
    public class UIManager : Manager<UIManager>
    {
        // sortingOrder을 관리하기 위한 변수
        int _order = 10;

        // 팝업 UI들을 담고있는 스택
        Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

        // WorldSpace UI들을 관리하는 HashSet
        HashSet<UI_WorldSpace> _worldSpaceUIs = new HashSet<UI_WorldSpace>();

        // Normal UI들을 관리하는 HashSet
        HashSet<UI_Base> _normalUIs = new HashSet<UI_Base>();

        // 각 씬마다 있는 고유한 UI
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
        public void SetCanvasWorldSpace(GameObject go)
        {
            Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
        }
        // 씬에 있는 고유한 UI를 보여주는 함수.
        // 씬이 시작될 때 한번 호출하면 될듯?
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

        // 팝업 UI를 띄워주는 함수
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
        public T ShowWorldSpaceUI<T>(string name = null) where T : UI_WorldSpace
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = ResourceManager.Instance.Instantiate($"TestKjh/{name}");
            T ui = Util.GetOrAddComponent<T>(go);
            _worldSpaceUIs.Add(ui);

            go.transform.SetParent(Root.transform);

            return ui;
        }
        public T ShowNormalUI<T>(string name = null) where T : UI_Base
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = ResourceManager.Instance.Instantiate($"UI/Normal/{name}");
            T ui = Util.GetOrAddComponent<T>(go);
            _normalUIs.Add(ui);

            go.transform.SetParent(Root.transform);

            return ui;
        }
        // 팝업 UI를 닫아주는 함수
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
        // 모든 팝업 UI를 닫아주는 함수
        public void CloseAllPopupUI()
        {
            while (_popupStack.Count > 0)
                ClosePopupUI();
        }
        public void CloseWorldSpaceUI(UI_WorldSpace ui)
        {
            if (_worldSpaceUIs.Count == 0)
                return;
            if (ui == null) return;
            _worldSpaceUIs.Remove(ui);
            ResourceManager.Instance.Destroy(ui.gameObject);
        }
        public void CloseAllWorldSpaceUI()
        {
            foreach(var ui in _worldSpaceUIs)
            {
                CloseWorldSpaceUI(ui);
            }
        }
        public void Clear()
        {
            CloseAllPopupUI();
            CloseAllWorldSpaceUI();
            SceneUI = null;
        }
    }
}

