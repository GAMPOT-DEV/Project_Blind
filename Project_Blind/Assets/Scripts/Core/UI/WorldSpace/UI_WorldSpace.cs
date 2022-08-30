using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blind;

namespace Blind
{
    public class UI_WorldSpace : UI_Base
    {
        public GameObject Owner = null;
        public override void Init()
        {
            UIManager.Instance.SetCanvasWorldSpace(gameObject);
        }
        public virtual void CloseWorldSpaceUI()
        {
            UIManager.Instance.CloseWorldSpaceUI(this);
        }
        public virtual void SetPosition(Vector3 pos, Vector3 offset)
        {
            gameObject.transform.position = pos + offset;
        }
        protected override void Awake()
        {
            Init();
        }
    }
}

