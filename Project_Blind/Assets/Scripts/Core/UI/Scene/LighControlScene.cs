using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class LighControlScene : MonoBehaviour
    {
        void Start()
        {
            UIManager.Instance.ShowNormalUI<UI_LightControl>();
        }
    }
}


