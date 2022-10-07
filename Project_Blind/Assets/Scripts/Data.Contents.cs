using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blind;
using System;

namespace Data
{
    /// <summary>
    /// Json의 값에 대응되게 클래스를 만들면 DataManager을 통해 파싱해서
    /// 딕셔너리를 만들어준다.
    /// </summary>
    #region Conversation
    [Serializable]
    public class Conversation
    {
        public string title;
        public List<Script> KOR;
        public List<Script> ENG;
    }
    [Serializable]
    public class Script
    {
        public string script;
    }
    [Serializable]
    public class ConversationData : ILoader<string, Conversation> // Key : String, Value : Conversation
    {
        // FromJson<Data.ConversationData>을 하면 자동으로 리스트로 파싱해서 여기에 저장됨
        public List<Conversation> conversations = new List<Conversation>();

        // 자동으로 파싱된 리스트를 딕셔너리로 변환해서 반환함
        public Dictionary<string, Conversation> MakeDict()
        {
            Dictionary<string, Conversation> dict = new Dictionary<string, Conversation>();
            // Key값이 title, Value값이 Conversation인 딕셔너리를 만들어서 반환
            foreach (Conversation conversation in conversations)
                dict.Add(conversation.title, conversation);
            return dict;
        }
    }
    #endregion
    #region Clue
    [Serializable]
    public class Clue
    {
        public int id;
        public string name;
        public string description;
        public string iconPath;
    }
    [Serializable]
    public class ClueData : ILoader<int, Clue>
    {
        public List<Clue> clues = new List<Clue>();
        public Dictionary<int, Clue> MakeDict()
        {
            Dictionary<int, Clue> dict = new Dictionary<int, Clue>();
            foreach (Clue clue in clues)
                dict.Add(clue.id, clue);
            return dict;
        }
    }
    #endregion
    #region Bag
    [Serializable]
    public class BagItem
    {
        public int id;
        public int cost;
        public string name;
        public string shortDescription;
        public string longDescription;
        public string effectDescription;
        public string iconPath;
    }
    public class BagItemData : ILoader<int, BagItem>
    {
        public List<BagItem> bagItems = new List<BagItem>();
        public Dictionary<int, BagItem> MakeDict()
        {
            Dictionary<int, BagItem> dict = new Dictionary<int, BagItem>();
            foreach(BagItem bagItem in bagItems)
                dict.Add(bagItem.id, bagItem);
            return dict;
        }
    }
    #endregion
    #region Talisman
    [Serializable]
    public class TalismanItem
    {
        public int id;
        public int abilityValue;
        public int cost;
        public string name;
        public string description;
        public string iconPath;
        public Define.TalismanAbility ability;
    }
    [Serializable]
    public class TalismanItemData : ILoader<int, TalismanItem>
    {
        public List<TalismanItem> talismanItems = new List<TalismanItem>();
        public Dictionary<int, TalismanItem> MakeDict()
        {
            Dictionary<int, TalismanItem> dict = new Dictionary<int, TalismanItem>();
            foreach(TalismanItem talismanItem in talismanItems)
                dict.Add(talismanItem.id, talismanItem);
            return dict;
        }
    }
    #endregion
}