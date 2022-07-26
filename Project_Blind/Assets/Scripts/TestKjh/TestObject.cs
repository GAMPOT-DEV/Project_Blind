using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blind;

namespace Blind
{
    public class TestObject : MonoBehaviour
    {
        InteractionAble _interaction;
        void Awake()
        {
            _interaction = gameObject.GetOrAddComponent<InteractionTest>();
        }
    }
}

