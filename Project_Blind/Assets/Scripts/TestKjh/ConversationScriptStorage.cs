using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class ConversationScriptStorage : MonoBehaviour
    {
        static ConversationScriptStorage _instance = null;
        public static ConversationScriptStorage Instance
        {
            get
            {
                Init();
                return _instance;
            }
        }
        static void Init()
        {
            if (_instance == null)
            {
                GameObject go = new GameObject() { name = "@ConversationStorage" };
                go.AddComponent<ConversationScriptStorage>();

                DontDestroyOnLoad(go);
                _instance = go.GetComponent<ConversationScriptStorage>();
                _instance.Init_Dict();
            }
        }
        private void Start()
        {
            Init();
        }
        Dictionary<string, List<string>> _dict = new Dictionary<string, List<string>>();
        public List<string> GetConversation(string text)
        {
            List<string> list = null;
            _dict.TryGetValue(text, out list);

            return list;
        }
        void Init_Dict()
        {
            _dict.Add("Test", new List<string>()
            {
                "안녕하세요",
                "반갑습니다",
                "수고하세요"
            });
        }
    }
}

