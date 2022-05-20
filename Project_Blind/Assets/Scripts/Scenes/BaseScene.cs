using Blind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// BaseScene 클래스입니다. 각 씬 스크립트들은 이 클래스를 상속받습니다.
/// </summary>

namespace Blind
{
    public abstract class BaseScene : MonoBehaviour
    {
        public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

        void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
            if (obj == null)
                ResourceManager.Instance.Instantiate("UI/EventSystem").name = "@EventSystem";
        }

        public abstract void Clear();
    }
}

