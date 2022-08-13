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

        #region Clue
        public List<ClueInfo> clueInfos = new List<ClueInfo>();
        #endregion
    }
    [Serializable]
    public class ClueInfo
    {
        public int slot;
        public int itemId;
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
    }
}

