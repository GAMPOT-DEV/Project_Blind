using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Clue_Item : UI_Base
    {
        Dictionary<int, Data.Clue> _cludData;
        enum Images
        {
            Image_ItemIcon,
        }
        public int ItemId { get;private set; }
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            _cludData = DataManager.Instance.ClueDict;

            Get<Image>((int)Images.Image_ItemIcon).gameObject.BindEvent(PushItemIcon, Define.UIEvent.Click);
        }
        public void SetItem(int itemId)
        {
            ItemId = itemId;
            Data.Clue clue;
            _cludData.TryGetValue(itemId, out clue);
            if (clue == null)
            {
                Get<Image>((int)Images.Image_ItemIcon).sprite = null;
            }
            else
            {
                Sprite sprite = ResourceManager.Instance.Load<Sprite>(clue.iconPath);
                Get<Image>((int)Images.Image_ItemIcon).sprite = sprite;
            }
        }
        public void PushItemIcon()
        {
            Debug.Log(gameObject.name);
        }
    }
}

