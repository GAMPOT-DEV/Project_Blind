using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Blind.InventorySystem
{
    /// <summary> 장비 아이템</summary>
    public abstract class EquipmentItem : Item
    {
        public EquipmentItemData EquipmentData { get; private set; }

        /// <summary> 현재 내구도 </summary>
        public int Durability
        {
            get => _durability;
            set
            {
                if(value < 0) value = 0;
                if(value > EquipmentData.MaxDurability)
                    value = EquipmentData.MaxDurability;

                _durability = value;
            }
        }
        private int _durability;

        public EquipmentItem(EquipmentItemData data) : base(data)
        {
            EquipmentData = data;
            Durability = data.MaxDurability;
        }
    }
}