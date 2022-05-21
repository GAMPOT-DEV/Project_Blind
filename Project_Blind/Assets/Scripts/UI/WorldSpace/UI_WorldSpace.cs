using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blind;

namespace Blind
{
    public class UI_WorldSpace : UI_Base
    {
        public override void Init()
        {
            UIManager.Instance.SetCanvasWorldSpace(gameObject);
        }
        public virtual void CloseWorldSpaceUI()
        {
            UIManager.Instance.CloseWorldSpaceUI(this);
        }
        public virtual void SetPosition(Transform obj)
        {
            gameObject.transform.position = obj.position + Vector3.up * 3;
        }
    }
}

