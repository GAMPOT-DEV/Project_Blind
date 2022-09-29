using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Blind
{
    [Serializable]
    public partial class GameData
    {
        #region Settings
        // Sound
        public float mastetVolume = 1.0f;
        public float bgmVolume = 1.0f;
        public float effectVolume = 1.0f;

        // Effect
        public bool vibration = true;
        public float motionEffect = 1.0f;

        //Key

        // Resolution
        public int resolutionIndex = 2;
        public bool windowMode = false;
        #endregion

        public int money = 0;
        public int caveClear = 0;

        #region Clue
        public List<ClueInfo> clueInfos = new List<ClueInfo>();
        #endregion

        public List<TalismanInfo> talismanInfos = new List<TalismanInfo>();
        public int currEquipCnt = 0;

        public List<BagItemInfo> bagItemInfos = new List<BagItemInfo>();
    }
    [Serializable]
    public class ClueInfo
    {
        public int slot;
        public int itemId;
    }
    [Serializable]
    public class TalismanInfo
    {
        public int slot;
        public int itemId;
        public bool equiped;
    }
    [Serializable]
    public class BagItemInfo
    {
        public int slot;
        public int itemId;
        public int itemCnt;
    }
    public class PlayerCharacterData
    {
        public UnitHP Hp;
        public int CurrentWaveGage;
        public TransitionDestination.DestinationTag DestinationTag;

        public PlayerCharacterData(UnitHP hp, int currentWaveGage)
        {
            Hp = new UnitHP(hp.GetMaxHP(),hp.GetHP());
            CurrentWaveGage = currentWaveGage;
        }
    }
    public partial class GameData
    {
        #region ClueDict
        public Dictionary<int, ClueInfo> ClueInfoBySlot { get; private set; } = new Dictionary<int, ClueInfo>();
        public Dictionary<int, ClueInfo> ClueInfoById { get; private set; } = new Dictionary<int, ClueInfo>();
        public void MakeClueDict()
        {
            foreach(ClueInfo clueInfo in clueInfos)
            {
                ClueInfoBySlot.Add(clueInfo.slot, clueInfo);
                ClueInfoById.Add(clueInfo.itemId, clueInfo);
            }
        }
        public void AddClueItem(ClueInfo clue)
        {
            clueInfos.Add(clue);
            ClueInfoBySlot.Add(clue.slot, clue);
            ClueInfoById.Add(clue.itemId, clue);
        }
        public void DeleteClueItem(ClueInfo clue)
        {
            clueInfos.Remove(clue);
            ClueInfoBySlot.Remove(clue.slot);
            ClueInfoById.Remove(clue.itemId);
        }
        public void ClearClueData()
        {
            clueInfos.Clear();
            ClueInfoBySlot.Clear();
            ClueInfoById.Clear();
        }
        #endregion
        #region Talisman
        public Dictionary<int, TalismanInfo> TalismanInfoBySlot { get; private set; } = new Dictionary<int, TalismanInfo>();
        public Dictionary<int, TalismanInfo> TalismanInfoById { get; private set; } = new Dictionary<int, TalismanInfo>();
        public void MakeTalismanDict()
        {
            foreach (TalismanInfo talismanInfo in talismanInfos)
            {
                TalismanInfoBySlot.Add(talismanInfo.slot, talismanInfo);
                TalismanInfoById.Add(talismanInfo.itemId, talismanInfo);
            }
        }
        public void AddTalismanItem(TalismanInfo talisman)
        {
            talismanInfos.Add(talisman);
            TalismanInfoBySlot.Add(talisman.slot, talisman);
            TalismanInfoById.Add(talisman.itemId, talisman);
        }
        public void DeleteTalismanItem(TalismanInfo talisman)
        {
            talismanInfos.Remove(talisman);
            TalismanInfoBySlot.Remove(talisman.slot);
            TalismanInfoById.Remove(talisman.itemId);
        }
        public void EquipTalismanItem(TalismanInfo talisman)
        {
            talisman.equiped = true;
            currEquipCnt++;
        }
        public void UnequipTalismanItem(TalismanInfo talisman)
        {
            talisman.equiped = false;
            currEquipCnt--;
        }
        public void ClearTalismanData()
        {
            talismanInfos.Clear();
            TalismanInfoBySlot.Clear();
            TalismanInfoById.Clear();
            currEquipCnt = 0;
        }
        #endregion
        #region BagItemDict
        public Dictionary<int, BagItemInfo> BagItemInfoBySlot { get; private set; } = new Dictionary<int, BagItemInfo>();
        public Dictionary<int, BagItemInfo> BagItemInfoById { get; private set; } = new Dictionary<int, BagItemInfo>();
        public void MakeBagItemDict()
        {
            foreach (BagItemInfo bagItemInfo in bagItemInfos)
            {
                BagItemInfoBySlot.Add(bagItemInfo.slot, bagItemInfo);
                BagItemInfoById.Add(bagItemInfo.itemId, bagItemInfo);
            }
        }
        public void AddBagItem(BagItemInfo bagItem)
        {
            bagItemInfos.Add(bagItem);
            BagItemInfoBySlot.Add(bagItem.slot, bagItem);
            BagItemInfoById.Add(bagItem.itemId, bagItem);
        }
        public void AddBagItem(int itemId, int cnt)
        {
            BagItemInfo item = BagItemInfoById[itemId];
            item.itemCnt += cnt;
        }
        public void DeleteBagItem(BagItemInfo item)
        {
            bagItemInfos.Remove(item);
            BagItemInfoBySlot.Remove(item.slot);
            BagItemInfoById.Remove(item.itemId);
        }
        public void DeleteBagItem(BagItemInfo item, int cnt)
        {
            item.itemCnt -= cnt;
        }
        public void OneIndexForwardBag(int start, int end)
        {
            for(int i = start; i < end; i++)
            {
                BagItemInfoBySlot[i].slot = i - 1;
                BagItemInfoBySlot[i - 1] = BagItemInfoBySlot[i];
            }
            BagItemInfoBySlot.Remove(end - 1);
        }
        public void ClearBagData()
        {
            bagItemInfos.Clear();
            BagItemInfoBySlot.Clear();
            BagItemInfoById.Clear();
        }
        #endregion

        #region PlayerCharater
        public PlayerCharacterData PlayerCharacterData { get; set; }

        #endregion
    }
}

