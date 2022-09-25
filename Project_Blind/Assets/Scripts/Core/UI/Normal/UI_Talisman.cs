using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Talisman : UI_Base
    {
        enum Images
        {
            ItemPanel1,
            ItemPanel2,
            ItemPanel3,

            Image_TestGetItem1,
            Image_TestGetItem2,
            Image_TestGetItem3,
            Image_TestGetItem4,
            Image_TestGetItem5,
            Image_TestGetItem6,
            Image_TestGetItem7,

            Image_TestClear,
        }
        enum Texts
        {

        }
        enum Buttons
        {
            Button_Refresh
        }
        [SerializeField] private Sprite basicPanel;
        Dictionary<int, Data.TalismanItem> _talismanData;
        public static int Size { get; set; }
        public List<UI_Talisman_Item> Items { get; } = new List<UI_Talisman_Item>();
        GameObject grid;
        const int EQUIP_SIZE = 3;
        int[] EquipItem = new int[EQUIP_SIZE];
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Button>(typeof(Buttons));

            _talismanData = DataManager.Instance.TalismanItemDict;

            Items.Clear();

            // 일단 아이템 슬롯들을 전부 날려준 뒤
            grid = GetComponentInChildren<GridLayoutGroup>().gameObject;
            foreach (Transform child in grid.transform)
                Destroy(child.gameObject);

            // SIZE만큼 슬롯들을 만들어서 Items에서 관리하도록 한다.
            Size = DataManager.Instance.GameData.talismanInfos.Count;
            for (int i = 0; i < Size; i++)
            {
                GameObject go = ResourceManager.Instance.Instantiate("UI/Normal/UI_Talisman_Item", grid.transform);
                UI_Talisman_Item item = go.GetOrAddComponent<UI_Talisman_Item>();
                Items.Add(item);
            }
            for(int i = 0; i < Size; i++)
            {
                if (DataManager.Instance.GameData.talismanInfos[i].equiped)
                {
                    for(int j = 0; j < EQUIP_SIZE; j++)
                    {
                        if (EquipItem[j] != 0) continue;
                        EquipItem[j] = DataManager.Instance.GameData.talismanInfos[i].itemId;
                        break;
                    }
                }
            }

            // UI를 데이터 정보에 맞게 갱신해준다.
            RefreshUI();

            TestInit();

            Get<Button>((int)Buttons.Button_Refresh).gameObject.BindEvent(PushRefreshButton);
        }
        void TestInit()
        {
            Get<Image>((int)Images.Image_TestGetItem1).gameObject.BindEvent(() => PushTestImage(Define.TalismanItem.TestTalisman1), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem2).gameObject.BindEvent(() => PushTestImage(Define.TalismanItem.TestTalisman2), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem3).gameObject.BindEvent(() => PushTestImage(Define.TalismanItem.TestTalisman3), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem4).gameObject.BindEvent(() => PushTestImage(Define.TalismanItem.TestTalisman4), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem5).gameObject.BindEvent(() => PushTestImage(Define.TalismanItem.TestTalisman5), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem6).gameObject.BindEvent(() => PushTestImage(Define.TalismanItem.TestTalisman6), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetItem7).gameObject.BindEvent(() => PushTestImage(Define.TalismanItem.TestTalisman7), Define.UIEvent.Click);

            Get<Image>((int)Images.Image_TestClear).gameObject.BindEvent(PushTestClear, Define.UIEvent.Click);
        }
        // 현재 가지고 있는 단서들의 정보를 이용해서 UI를 갱신해준다.
        public void RefreshUI()
        {
            Size = DataManager.Instance.GameData.talismanInfos.Count;
            grid.GetComponent<RectTransform>().sizeDelta = new Vector2(800f, 220f * Size);
            for (int i = 0; i < Size; i++)
            {
                int itemId = -1;

                TalismanInfo info = null;
                DataManager.Instance.GameData.TalismanInfoBySlot.TryGetValue(i, out info);
                if (info != null)
                    itemId = info.itemId;

                // 각각의 슬롯에 아이템 ID를 넘겨줘서 갱신한다.
                // 위에서 찾지 못하면 ID = -1이고 ID가 -1인 아이템은 없기 때문에 
                // 자동으로 null로 할당된다.
                Items[i].SetItem(itemId, i, this);
            }
            for(int i = 0; i < EQUIP_SIZE; i++)
            {
                if (EquipItem[i] == 0)
                {
                    Get<Image>(i).sprite = basicPanel;
                }
                else
                {
                    Sprite sprite = ResourceManager.Instance.Load<Sprite>(_talismanData[EquipItem[i]].iconPath);
                    Get<Image>(i).sprite = sprite;
                }
            }
        }
        private void PushTestImage(Define.TalismanItem itemEnum)
        {
            // 게임데이터에 아이템을 추가를 시도한다.
            bool result = DataManager.Instance.AddTalismanItem(itemEnum);
            // 만약 성공한다면 슬롯을 하나 더 만들어주고 UI를 새로고침해준다.
            if (result == true)
            {
                GameObject go = ResourceManager.Instance.Instantiate("UI/Normal/UI_Talisman_Item", grid.transform);
                UI_Talisman_Item item = go.GetOrAddComponent<UI_Talisman_Item>();
                Items.Add(item);
            }
            RefreshUI();
        }
        private void PushTestClear()
        {
            DataManager.Instance.ClearTalismanData();
            Items.Clear();
            // 단서 슬롯들을 전부 날려주고 UI를 새로고침해준다.
            foreach (Transform child in grid.transform)
                Destroy(child.gameObject);

            for (int i = 0; i < EQUIP_SIZE; i++)
            {
                EquipItem[i] = 0;
            }
            //ShowDetailDesc(-1);
            RefreshUI();
        }
        public void EquipTalisman(int itemId)
        {
            for(int i = 0; i < EQUIP_SIZE; i++)
            {
                if (EquipItem[i] != 0) continue;
                EquipItem[i] = itemId;
                break;
            }
            RefreshUI();
        }
        public void UnequipTalisman(int itemId)
        {
            for(int i = 0; i < EQUIP_SIZE; i++)
            {
                if (EquipItem[i] == itemId)
                {
                    EquipItem[i] = 0;
                    break;
                }
            }
            RefreshUI();
        }
        private void PushRefreshButton()
        {
            for(int i = 0; i < EQUIP_SIZE; i++)
            {
                if (EquipItem[i] == 0) continue;
                DataManager.Instance.EquipOrUnequipTalisman((Define.TalismanItem)EquipItem[i]);
                EquipItem[i] = 0;
            }
            RefreshUI();
        }
    }
}