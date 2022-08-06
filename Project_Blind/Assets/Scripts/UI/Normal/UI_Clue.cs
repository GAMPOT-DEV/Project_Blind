using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Clue : UI_Base
    {
        enum Images
        {
            Image_Close,
        }
        const int SIZE = 7;
        public List<UI_Clue_Item> Items { get; } = new List<UI_Clue_Item>();
        List<ClueInfo> infos;
        public override void Init()
        {
            Bind<Image>(typeof(Images));

            Items.Clear();
            infos = DataManager.Instance.GameData.clueSlotInfos;

            GameObject grid = transform.Find("Background").transform.Find("ItemGrid").gameObject;
            foreach (Transform child in grid.transform)
                Destroy(child.gameObject);

            for(int i = 0; i < SIZE; i++)
            {
                GameObject go = ResourceManager.Instance.Instantiate("UI/Normal/UI_Clue_Item", grid.transform);
                UI_Clue_Item item = go.GetOrAddComponent<UI_Clue_Item>();
                Items.Add(item);
            }
            RefreshUI();

            Get<Image>((int)Images.Image_Close).gameObject.BindEvent(PushCloseButton, Define.UIEvent.Click);
        }
        public void RefreshUI()
        {
            for(int i = 0; i < SIZE; i++)
            {
                int itemId = -1;
                foreach(ClueInfo info in infos)
                {
                    if (i == info.slot)
                    {
                        itemId = info.itemId;
                        break;
                    }
                }
                Items[i].SetItem(itemId);
            }
        }
        public void GetItem(ClueInfo info)
        {
            foreach(ClueInfo info2 in infos)
            {
                if (info.itemId == info2.itemId)
                    return;
            }
        }
        private void PushCloseButton()
        {
            //DataManager.Instance.GameData.clueSlotInfos.Clear();
            UIManager.Instance.CloseNormalUI(this);
        }
    }
}
