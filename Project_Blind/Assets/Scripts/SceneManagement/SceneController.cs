using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blind
{
    /// <summary>
    /// 씬의 트랜지션, 재시작 등을 관리하는 컨트롤러입니다.
    /// </summary>
    public class SceneController : Manager<SceneController>
    {
        public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
        protected override void Awake()
        {
            base.Awake();
        }
        public void LoadScene(Define.Scene type)
        {
            SceneManager.LoadScene(GetSceneName(type));
        }

        string GetSceneName(Define.Scene type)
        {
            string name = System.Enum.GetName(typeof(Define.Scene), type);
            return name;
        }

        public void Clear()
        {
            CurrentScene.Clear();
        }
    }
}
