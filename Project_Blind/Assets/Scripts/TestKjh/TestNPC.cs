using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class TestNPC : MonoBehaviour
    {
        InteractionAble _interactionConversation;
        InteractionAble _interaction;
        void Awake()
        {
            _interactionConversation = gameObject.GetOrAddComponent<ConversationTest>();
            _interaction = gameObject.GetOrAddComponent<InteractionTest>();
        }
    }
}

