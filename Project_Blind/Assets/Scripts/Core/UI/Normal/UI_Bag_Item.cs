using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Bag_Item : UI_Base
    {
        private UI_Bag _owner;
        Dictionary<int, Data.BagItem> _bagItemData;
        public Data.BagItem BagItem { get;private set; }

        enum Images
        {
            Image_ItemIcon
        }
        enum Texts
        {
            Text_ItemName,
            Text_ItemShortDesc,
            Text_ItemCnt
        }
        public int ItemId { get; private set; }
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Text>(typeof(Texts));
            _bagItemData = DataManager.Instance.BagItemDict;
            Get<Image>((int)Images.Image_ItemIcon).gameObject.BindEvent(PushItemIcon, Define.UIEvent.Click);
        }

        public void SetItem(int itemId, UI_Bag owner)
        {
            _owner = owner;
            ItemId = itemId;
            Data.BagItem bagItem;
            _bagItemData.TryGetValue(itemId, out bagItem);
            BagItem = bagItem;
            if (bagItem == null)
            {
                Get<Image>((int)Images.Image_ItemIcon).sprite = null;
            }
            else
            {
                Sprite sprite = ResourceManager.Instance.Load<Sprite>(BagItem.iconPath);
                Get<Image>((int)Images.Image_ItemIcon).sprite = sprite;
            }
            SetText();
        }
        private void SetText()
        {
            Get<Text>((int)Texts.Text_ItemName).text = BagItem.name;
            Get<Text>((int)Texts.Text_ItemShortDesc).text = BagItem.shortDescription;
            int cnt = DataManager.Instance.GameData.BagItemInfoById[ItemId].itemCnt;
            Get<Text>((int)Texts.Text_ItemCnt).text = $"{cnt} 개"; 
        }
        private void PushItemIcon()
        {
            _owner.ShowDetailDesc(ItemId);
        }
    }
}

