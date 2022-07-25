using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Blind.InventorySystem
{
    /*
        [상속 구조]
        Item : 기본 아이템
            - EquipmentItem : 장비 아이템
            - CountableItem : 수량이 존재하는 아이템
    */
    public abstract class Item
    {
        public ItemData Data { get; private set; }

        public Item(ItemData data) => Data = data;
    }
}