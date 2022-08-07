using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Clue_Item : UI_Base
    {
        Dictionary<int, Data.Clue> _cludData;
        public Data.Clue Clue { get; private set; }
        enum Images
        {
            Image_ItemIcon,
        }
        enum Texts
        {
            Text_ClueName,
            Text_ClueDesc,
        }
        public int ItemId { get;private set; }
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Text>(typeof(Texts));
            _cludData = DataManager.Instance.ClueDict;
        }
        public void SetItem(int itemId)
        {
            ItemId = itemId;
            Data.Clue clue;
            _cludData.TryGetValue(itemId, out clue);
            Clue = clue;
            if (Clue == null)
            {
                Get<Image>((int)Images.Image_ItemIcon).sprite = null;
            }
            else
            {
                Sprite sprite = ResourceManager.Instance.Load<Sprite>(Clue.iconPath);
                Get<Image>((int)Images.Image_ItemIcon).sprite = sprite;
            }
            SetText();
        }
        public void PushItemIcon()
        {
            Debug.Log(gameObject.name);
        }
        private void SetText()
        {
            Get<Text>((int)Texts.Text_ClueName).text = Clue.name;
            Get<Text>((int)Texts.Text_ClueDesc).text = Clue.description;
        }
    }
}

