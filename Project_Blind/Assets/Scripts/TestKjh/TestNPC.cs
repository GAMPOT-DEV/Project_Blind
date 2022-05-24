using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class TestNPC : MonoBehaviour
    {
        InteractionAble _interaction;
        void Awake()
        {
            _interaction = gameObject.GetOrAddComponent<ConversationTest>();
        }
    }
}

