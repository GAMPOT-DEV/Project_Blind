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
        public Dictionary<int, Data.TalismanItem> TalismanItemDict { get; private set; } = new Dictionary<int, Data.TalismanItem>();

        private PlayerCharacterData _playerCharacterData = null;
        public PlayerCharacterData PlayerCharacterDataValue
        {
            get => _playerCharacterData;
            set
            {
                _playerCharacterData = value;
                GameData.PlayerCharacterData = _playerCharacterData;
                SaveGameData();
            }
        }
        
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
            TalismanItemDict = LoadJson<Data.TalismanItemData, int, Data.TalismanItem>("TalismanItemData").MakeDict();
        }
        Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
        {
            TextAsset textAsset = ResourceManager.Instance.Load<TextAsset>($"Data/{path}");
            return JsonUtility.FromJson<Loader>(textAsset.text);
        }

        #region Load, Save Data
        string FileName = "/GameData1.json";
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
        private void LoadGameData()
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
            _gameData.MakeTalismanDict();
            _gameData.MakeBagItemDict();
            Debug.Log("Load");
        }
        public void SaveGameData()
        {
            string JsonData = JsonUtility.ToJson(GameData);
            string filePath = Application.persistentDataPath + FileName;
            File.WriteAllText(filePath, JsonData);
        }
        #endregion
        public bool AddClueItem(Define.ClueItem itemId)
        {
            int id = (int)itemId;

            ClueInfo clue = null;
            GameData.ClueInfoById.TryGetValue(id, out clue);
            if (clue != null)
                return false;

            clue = new ClueInfo() { itemId = id, slot = GameData.clueInfos.Count };
            GameData.AddClueItem(clue);
            SaveGameData();
            return true;
        }
        public void DeleteClueItem(Define.ClueItem itemId)
        {
            return;

            // 수정 필요

            //int id = (int)itemId;

            //ClueInfo clue = null;
            //_gameData.ClueInfoById.TryGetValue(id, out clue);
            //if (clue == null)
            //    return;

            //_gameData.DeleteClueItem(clue);
            //SaveGameData();
        }
        public bool HaveClueItem(Define.ClueItem itemId)
        {
            int id = (int)itemId;
            ClueInfo clue;
            GameData.ClueInfoById.TryGetValue(id, out clue);
            if (clue == null) return false;
            return true;
        }
        public void ClearClueData()
        {
            GameData.ClearClueData();
            SaveGameData();
        }
        #region Talisman
        public bool AddTalismanItem(Define.TalismanItem itemId)
        {
            int id = (int)itemId;

            TalismanInfo talisman = null;
            GameData.TalismanInfoById.TryGetValue(id, out talisman);
            if (talisman != null)
                return false;

            talisman = new TalismanInfo() { itemId = id, equiped = false, slot = GameData.talismanInfos.Count };
            GameData.AddTalismanItem(talisman);
            SaveGameData();
            return true;
        }
        public void DeleteTalismanItem(Define.TalismanItem itemId)
        {
            return;

            // 수정 필요

            //int id = (int)itemId;

            //TalismanInfo talisman = null;
            //_gameData.TalismanInfoById.TryGetValue(id, out talisman);
            //if (talisman == null)
            //    return;

            //_gameData.DeleteTalismanItem(talisman);
            //SaveGameData();
        }
        public bool HaveTalismanItem(Define.TalismanItem itemId)
        {
            int id = (int)itemId;
            TalismanInfo talisman;
            GameData.TalismanInfoById.TryGetValue(id, out talisman);
            if (talisman == null) return false;
            return true;
        }
        public bool EquipOrUnequipTalisman(Define.TalismanItem itemId)
        {
            int id = (int)itemId;
            TalismanInfo talisman;
            GameData.TalismanInfoById.TryGetValue(id, out talisman);
            if (talisman == null) return false;

            if (talisman.equiped == false)
            {
                if (GameData.currEquipCnt >= 3) return false;
                GameData.EquipTalismanItem(talisman);
                SaveGameData();
                return true;
            }

            GameData.UnequipTalismanItem(talisman);
            SaveGameData();
            return true;
        }
        public void ClearTalismanData()
        {
            GameData.ClearTalismanData();
            SaveGameData();
        }
        #endregion
        #region Bag
        public bool AddBagItem(Define.BagItem itemId, int cnt = 1)
        {
            int id = (int)itemId;

            BagItemInfo item = null;
            GameData.BagItemInfoById.TryGetValue(id, out item);

            if (item != null)
            {
                GameData.AddBagItem(id, cnt);
                RefreshFieldUI();
                SaveGameData();
                return false;
            }

            item = new BagItemInfo() { itemId = id, slot = GameData.bagItemInfos.Count, itemCnt = cnt };
            GameData.AddBagItem(item);
            RefreshFieldUI();
            SaveGameData();
            return true;
        }
        public bool DeleteBagItem(Define.BagItem itemId, int cnt = 1)
        {
            int id = (int)itemId;

            BagItemInfo item = null;
            GameData.BagItemInfoById.TryGetValue(id, out item);
            if (item == null)
                return false;

            if (cnt < 1) return false;

            if (item.itemCnt < cnt)
                return false;

            if (item.itemCnt == cnt)
            {
                int slot = item.slot;
                GameData.DeleteBagItem(item);
                OneIndexForwardBag(slot + 1, GameData.bagItemInfos.Count + 1);
            }
            else
                GameData.DeleteBagItem(item, cnt);

            RefreshFieldUI();
            SaveGameData();
            return true;
        }
        private void OneIndexForwardBag(int start, int end)
        {
            GameData.OneIndexForwardBag(start, end);
        }
        public void ClearBagData()
        {
            GameData.ClearBagData();
            RefreshFieldUI();
            SaveGameData();
        }
        public bool HaveBagItem(Define.BagItem itemId)
        {
            int id = (int)itemId;
            BagItemInfo item;
            GameData.BagItemInfoById.TryGetValue(id, out item);
            if (item == null) return false;
            return true;
        }
        #endregion
        private void RefreshFieldUI()
        {
            UI_FieldScene ui = FindObjectOfType<UI_FieldScene>();
            if (ui != null) ui.RefreshItemCnt();
        }

        public bool AddMoney(int money)
        {
            if (money <= 0) return false;
            GameData.money += money;
            UI_FieldScene ui = FindObjectOfType<UI_FieldScene>();
            if(ui != null)
            {
                ui.DisplayMoney();
            }
            SaveGameData();
            return true;
        }
        public bool SubMoney(int money)
        {
            if (money <= 0) return false;
            if (GameData.money < money) return false;
            GameData.money -= money;
            UI_FieldScene ui = FindObjectOfType<UI_FieldScene>();
            if (ui != null)
            {
                ui.DisplayMoney();
            }
            SaveGameData();
            return true;
        }
        public int GetMoney()
        {
            return GameData.money;
        }

        public void CaveOpen()
        {
            GameData.caveClear = 1;
            Debug.Log("cave opened");
        }
        public void ClearCaveData()
        {
            GameData.caveClear = 0;

        }
        public int GetClearCave()
        {
            return GameData.caveClear;
        }

        public void ForestCutSceneDone()
        {
            GameData.forestCutSceneData = 1;
        }

        public void ClearForestCutSceneData()
        {
            GameData.forestCutSceneData = 0;
        }

        public int GetForestCutSceneData()
        {
            return GameData.forestCutSceneData;
        }


    }
}

