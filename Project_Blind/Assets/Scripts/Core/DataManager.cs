using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// Json을 파싱하는것을 도와주는 클래스
    /// </summary>
    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }
    public class DataManager : Manager<DataManager>
    {
        public Dictionary<string, Data.Conversation> ConversationDict { get; private set; } = new Dictionary<string, Data.Conversation>();
        public Dictionary<int, Data.Clue> ClueDict { get; private set; } = new Dictionary<int, Data.Clue>();
        public Dictionary<int, Data.BagItem> BagItemDict { get; private set; } = new Dictionary<int, Data.BagItem>();
        protected override void Awake()
        {
            base.Awake();
            Init();
        }
        public void Init()
        {
            ConversationDict = LoadJson<Data.ConversationData, string, Data.Conversation>("ConversationData").MakeDict();
            ClueDict = LoadJson<Data.ClueData, int, Data.Clue>("ClueData").MakeDict();
            BagItemDict = LoadJson<Data.BagItemData, int, Data.BagItem>("BagItemData").MakeDict();
        }
        Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
        {
            TextAsset textAsset = ResourceManager.Instance.Load<TextAsset>($"Data/{path}");
            return JsonUtility.FromJson<Loader>(textAsset.text);
        }

        #region Load, Save Data
        string FileName = "/GameData.json";
        GameData _gameData;
        public GameData GameData
        {
            get
            {
                if (_gameData == null)
                {
                    LoadGameData();
                    SaveGameData();
                }
                return _gameData;
            }
        }
        public void LoadGameData()
        {
            string filePath = Application.persistentDataPath + FileName;

            Debug.Log(Application.persistentDataPath);

            if (File.Exists(filePath))
            {
                string JsonData = File.ReadAllText(filePath);
                _gameData = JsonUtility.FromJson<GameData>(JsonData);
            }
            else
            {
                _gameData = new GameData();
            }

            _gameData.MakeClueDict();
            _gameData.MakeBagItemDict();
        }
        public void SaveGameData()
        {
            string JsonData = JsonUtility.ToJson(GameData);
            string filePath = Application.persistentDataPath + FileName;
            File.WriteAllText(filePath, JsonData);
        }
        #endregion
        public bool AddClueItem(int itemId)
        {
            ClueInfo clue = null;
            _gameData.ClueInfoById.TryGetValue(itemId, out clue);
            if (clue != null)
                return false;

            clue = new ClueInfo() { itemId = itemId, slot = UI_Clue.Size++ };
            _gameData.AddClueItem(clue);
            SaveGameData();
            return true;
        }
        public void DeleteClueItem(int itemId)
        {
            ClueInfo clue = null;
            _gameData.ClueInfoById.TryGetValue(itemId, out clue);
            if (clue == null)
                return;

            _gameData.DeleteClueItem(clue);
            SaveGameData();
        }
        public void ClearClueData()
        {
            _gameData.ClearClueData();
            SaveGameData();
        }

        public bool AddBagItem(int itemId, int cnt = 1)
        {
            BagItemInfo item = null;
            _gameData.BagItemInfoById.TryGetValue(itemId, out item);

            if (item != null)
            {
                _gameData.AddBagItem(itemId, cnt);
                SaveGameData();
                return false;
            }

            item = new BagItemInfo() { itemId = itemId, slot = UI_Bag.Size++, itemCnt = cnt };
            _gameData.AddBagItem(item);
            SaveGameData();
            return true;
        }
        public bool DeleteBagItem(int itemId, int cnt)
        {
            BagItemInfo item = null;
            _gameData.BagItemInfoById.TryGetValue(itemId, out item);
            if (item == null)
                return false;

            if (cnt < 1) return false;

            if (item.itemCnt < cnt)
                return false;

            if (item.itemCnt == cnt)
                _gameData.DeleteBagItem(item);
            else
                _gameData.DeleteBagItem(item, cnt);

            SaveGameData();
            return true;
        }
        public void OneIndexForwardBag(int start, int end)
        {
            _gameData.OneIndexForwardBag(start, end);
        }
        public void ClearBagData()
        {
            _gameData.ClearBagData();
            SaveGameData();
        }
    }
}

