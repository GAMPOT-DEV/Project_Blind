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
        protected override void Awake()
        {
            base.Awake();
            Init();
        }
        public void Init()
        {
            ConversationDict = LoadJson<Data.ConversationData, string, Data.Conversation>("ConversationData").MakeDict();
            ClueDict = LoadJson<Data.ClueData, int, Data.Clue>("ClueData").MakeDict();
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
                Debug.Log("Data Load Success!");
                string JsonData = File.ReadAllText(filePath);
                Debug.Log(JsonData);
                _gameData = JsonUtility.FromJson<GameData>(JsonData);
            }
            else
            {
                Debug.Log("New Data Created!");
                _gameData = new GameData();
            }

            _gameData.MakeClueDict();
        }
        public void SaveGameData()
        {
            string JsonData = JsonUtility.ToJson(GameData);
            string filePath = Application.persistentDataPath + FileName;
            File.WriteAllText(filePath, JsonData);
            Debug.Log("Save Completed!");
        }
        #endregion
        public bool GetClueItem(int itemId)
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
    }
}

