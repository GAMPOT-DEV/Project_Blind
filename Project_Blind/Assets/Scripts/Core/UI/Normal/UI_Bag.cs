using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Bag : UI_Base
    {
        enum Images
        {
            Image_Item,

            Image_TestGetItem1,
            Image_TestGetItem2,
            Image_TestGetItem3,
            Image_TestGetItem4,
            Image_TestGetItem5,
            Image_TestGetItem6,
            Image_TestGetItem7,
            Image_TestClearBag,

            Image_Dump,
        }
        enum Texts
        {
            Text_ItemNameDesc,
            Text_ItemLongDesc,
            Text_ItemEffectDesc,
        }

        Dictionary<int, Data.BagItem> _bagItemData;
        public static int Size { get; set; }
        public List<UI_Bag_Item> Items { get; } = new List<UI_Bag_Item>();

        [SerializeField]
        private GameObject grid;
        [SerializeField]
        private GameObject Blocker;

        private int _currSelectItemId = 0;
        private int _currSelectItemSlot;
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Text>(typeof(Texts));

            _bagItemData = DataManager.Instance.BagItemDict;
            Items.Clear();

            foreach (Transform child in grid.transform)
                Destroy(child.gameObject);
                

            Size = DataManager.Instance.GameData.bagItemInfos.Count;
            for (int i = 0; i < Size; i++)
            {
                GameObject go = ResourceManager.Instance.Instantiate("UI/Normal/UI_Bag_Item", grid.transform);
                UI_Bag_Item item = go.GetOrAddComponent<UI_Bag_Item>();
                Items.Add(item);
            }

            Get<Image>((int)Images.Image_Dump).gameObject.BindEvent(PushDumpButton, Define.UIEvent.Click);

            RefreshUI();

            TestInit();
        }
        public void RefreshUI()
        { 
            Size = DataManager.Instance.GameData.bagItemInfos.Count;

            if (_currSelectItemId == 0) Blocker.SetActive(true);
            else Blocker.SetActive(false);

            grid.GetComponent<RectTransform>().sizeDelta = new Vector2(750f, 220f * (float)Size);
            for (int i = 0; i < Size; i++)
            {
                int itemId = -1;

                BagItemInfo info;
                DataManager.Instance.GameData.BagItemInfoBySlot.TryGetValue(i, out info);
                if (info != null)
                    itemId = info.itemId;

                Items[i].SetItem(itemId, this);
            }
        }
        public void ShowDetailDesc(int itemId)
        {
            _currSelectItemId = itemId;

            if (_currSelectItemId == 0) Blocker.SetActive(true);
            else Blocker.SetActive(false);

            Data.BagItem item;
            _bagItemData.TryGetValue(itemId, out item);

            Sprite sprite = ResourceManager.Instance.Load<Sprite>(item.iconPath);

            string itemName = item.name;
            string longDesc = item.longDescription;
            string effectDesc = item.effectDescription;

            Get<Image>((int)Images.Image_Item).sprite = sprite;
            Get<Text>((int)Texts.Text_ItemNameDesc).text = itemName;
            Get<Text>((int)Texts.Text_ItemLongDesc).text = longDesc;
            Get<Text>((int)Texts.Text_ItemEffectDesc).text = effectDesc;
        }
        private void PushDumpButton()
        {
            if (Size == 0) return;
            if (_currSelectItemId == 0) return;
            UI_DumpBagItem dumpUI = UIManager.Instance.ShowNormalUI<UI_DumpBagItem>();
            dumpUI.AdditionalInit(this);
        }

        public void DumpItem(int cnt)
        {
            if (_currSelectItemId == 0) return;
            BagItemInfo info = DataManager.Instance.GameData.BagItemInfoById[_currSelectItemId];
            int lastCnt = info.itemCnt;
            int lastSlot = info.slot;
            DataManager.Instance.DeleteBagItem(_currSelectItemId, cnt);
            if (lastCnt == cnt)
            {
                GameObject go = Items[lastSlot].gameObject;
                Items.Remove(Items[lastSlot]);
                Destroy(go);
                DataManager.Instance.OneIndexForwardBag(lastSlot + 1, Size);
                _currSelectItemId = 0;
            }
            DataManager.Instance.SaveGameData();
            RefreshUI();
        }

        void TestInit()
        {
            Get<Image>((int)Images.Image_TestGetItem1).gameObject.BindEvent(() => PushTestImage(1), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem2).gameObject.BindEvent(() => PushTestImage(2), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem3).gameObject.BindEvent(() => PushTestImage(3), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem4).gameObject.BindEvent(() => PushTestImage(4), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem5).gameObject.BindEvent(() => PushTestImage(5), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem6).gameObject.BindEvent(() => PushTestImage(6), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem7).gameObject.BindEvent(() => PushTestImage(7), Define.UIEvent.Click);

            Get<Image>((int)Images.Image_TestClearBag).gameObject.BindEvent(PushTestClear, Define.UIEvent.Click);
        }
        private void PushTestImage(int id)
        {
            bool result = DataManager.Instance.AddBagItem(id, 1);
            if (result == true)
            {
                GameObject go = ResourceManager.Instance.Instantiate("UI/Normal/UI_Bag_Item", grid.transform);
                UI_Bag_Item item = go.GetOrAddComponent<UI_Bag_Item>();
                Items.Add(item);
            }
            RefreshUI();
        }
        private void PushTestClear()
        {
            DataManager.Instance.ClearBagData();
            Items.Clear();
            // 단서 슬롯들을 전부 날려주고 UI를 새로고침해준다.
            foreach (Transform child in grid.transform)
                Destroy(child.gameObject);

            _currSelectItemId = 0;
            //ShowDetailDesc(-1);
            RefreshUI();
        }
    }
}

