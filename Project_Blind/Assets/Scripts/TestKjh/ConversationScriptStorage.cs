using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class ConversationScriptStorage : Manager<ConversationScriptStorage>
    {
        // 이부분을 바꾸면 언어 바꿀 수 있음
        private int _languageNumber = (int)Define.Language.KOR;
        public int LanguageNumber
        {
            get { return _languageNumber; }
            private set { }
        }
        public void SetLanguageNum(Define.Language lang)
        {
            if (lang == Define.Language.MaxCount) return;
            _languageNumber = (int)lang;
        }
        protected override void Awake()
        {
            base.Awake();
            Init_Dict();
        }
        Dictionary<string, Dictionary<int, List<Data.Script>>> _dict = new Dictionary<string, Dictionary<int, List<Data.Script>>>();
        public Dictionary<int, List<Data.Script>> GetConversation(string text)
        {
            Dictionary<int, List<Data.Script>> dict = null;
            _dict.TryGetValue(text, out dict);

            return dict;
        }
        void Init_Dict()
        {
            var dict = DataManager.Instance.ConversationDict;
            Dictionary<int, List<Data.Script>> tmp = new Dictionary<int, List<Data.Script>>();
            foreach (var scripts in dict.Values)
            {
                tmp.Add((int)Define.Language.KOR, scripts.KOR);
                tmp.Add((int)Define.Language.ENG, scripts.ENG);
                _dict.Add(scripts.title, tmp);
            }
        }
    }
}

