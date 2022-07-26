using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind.InventorySystem
{
    /// <summary> 장비 - 무기 아이템 </summary>
    public class WeaponItem : EquipmentItem
    {
        public WeaponItem(WeaponItemData data) : base(data) { }
    }
}