using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Blind
{
    [Serializable]
    public class GameData
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
    }
}

