using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class TestNPC : MonoBehaviour
    {
        //ConversationTest _interactionConversation;
        InteractionTest _interaction;

        [SerializeField] private string InteractionText;
        [SerializeField] private Define.ScriptTitle ScriptTitle;
        [SerializeField] private Define.BagItem BagItem;
        [SerializeField] private Define.ClueItem ClueItem;
        void Awake()
        {
            //_interactionConversation = gameObject.GetOrAddComponent<ConversationTest>();
            //_interactionConversation.ScriptTitle = ScriptTitle;
            //_interactionConversation.BagItem = BagItem;
            //_interactionConversation.ClueItem = ClueItem;

            _interaction = gameObject.GetOrAddComponent<InteractionTest>();
            _interaction.InteractionText = InteractionText;
            _interaction.ScriptTitle = ScriptTitle;
            _interaction.BagItem = BagItem;
            _interaction.ClueItem = ClueItem;
        }
    }
}

