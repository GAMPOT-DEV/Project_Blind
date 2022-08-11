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
            Image_TestGetClue1,
            Image_TestGetClue2,
            Image_TestGetClue3,
            Image_TestGetClue4,
            Image_TestGetClue5,
            Image_TestGetClue6,
            Image_TestGetClue7,
            Image_TestClearClue,
        }
        enum Texts
        {
            Text_DetailDesc,
        }
        Dictionary<int, Data.Clue> _cludData;
        public static int Size { get; set; }
        public List<UI_Clue_Item> Items { get; } = new List<UI_Clue_Item>();
        List<ClueInfo> infos;
        GameObject grid;
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Text>(typeof(Texts));
            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.KeyInputEvents += HandleUIKeyInput;

            infos = DataManager.Instance.GameData.clueSlotInfos;
            _cludData = DataManager.Instance.ClueDict;

            Items.Clear();

            // 일단 아이템 슬롯들을 전부 날려준 뒤
            //GameObject grid = transform.Find("Background").transform.Find("ItemGrid").gameObject;
            grid = GetComponentInChildren<GridLayoutGroup>().gameObject;
            foreach (Transform child in grid.transform)
                Destroy(child.gameObject);

            // SIZE만큼 슬롯들을 만들어서 Items에서 관리하도록 한다.
            Size = DataManager.Instance.GameData.clueSlotInfos.Count;
            for (int i = 0; i < Size; i++)
            {
                GameObject go = ResourceManager.Instance.Instantiate("UI/Normal/UI_Clue_Item", grid.transform);
                UI_Clue_Item item = go.GetOrAddComponent<UI_Clue_Item>();
                Items.Add(item);
            }

            // UI를 데이터 정보에 맞게 갱신해준다.
            RefreshUI();

            Get<Image>((int)Images.Image_Close).gameObject.BindEvent(PushCloseButton, Define.UIEvent.Click);
            TestInit();
        }
        void TestInit()
        {
            Get<Image>((int)Images.Image_TestGetClue1).gameObject.BindEvent(() => PushTestImage(1), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetClue2).gameObject.BindEvent(() => PushTestImage(2), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetClue3).gameObject.BindEvent(() => PushTestImage(3), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetClue4).gameObject.BindEvent(() => PushTestImage(4), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetClue5).gameObject.BindEvent(() => PushTestImage(5), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetClue6).gameObject.BindEvent(() => PushTestImage(6), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestGetClue7).gameObject.BindEvent(() => PushTestImage(7), Define.UIEvent.Click);

            Get<Image>((int)Images.Image_TestClearClue).gameObject.BindEvent(PushTestClear, Define.UIEvent.Click);
        }
        private void HandleUIKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PushCloseButton();
                return;
            }
        }
        // 현재 가지고 있는 단서들의 정보를 이용해서 UI를 갱신해준다.
        public void RefreshUI()
        {
            Size = DataManager.Instance.GameData.clueSlotInfos.Count;
            grid.GetComponent<RectTransform>().sizeDelta = new Vector2(750f, 220f * (float)Size);
            for(int i = 0; i < Size; i++)
            {
                int itemId = -1;
                foreach(ClueInfo info in infos)
                {
                    // 아이템의 슬롯 정보가 현재 슬롯과 같을 때
                    if (i == info.slot)
                    {
                        itemId = info.itemId;
                        break;
                    }
                }
                // 각각의 슬롯에 아이템 ID를 넘겨줘서 갱신한다.
                // 위에서 찾지 못하면 ID = -1이고 ID가 -1인 아이템은 없기 때문에 
                // 자동으로 null로 할당된다.
                Items[i].SetItem(itemId, this);
            }
        }
        private void PushCloseButton()
        {
            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.CloseNormalUI(this);
        }
        private void PushTestImage(int id)
        {
            // 게임데이터에 아이템을 추가를 시도한다.
            bool result = DataManager.Instance.GetClueItem(id);
            // 만약 성공한다면 슬롯을 하나 더 만들어주고 UI를 새로고침해준다.
            if (result == true)
            {
                GameObject go = ResourceManager.Instance.Instantiate("UI/Normal/UI_Clue_Item", grid.transform);
                UI_Clue_Item item = go.GetOrAddComponent<UI_Clue_Item>();
                Items.Add(item);
            }
            RefreshUI();
        }
        private void PushTestClear()
        {
            DataManager.Instance.ClearClueData();
            Items.Clear();
            // 단서 슬롯들을 전부 날려주고 UI를 새로고침해준다.
            foreach (Transform child in grid.transform)
                Destroy(child.gameObject);

            ShowDetailDesc(-1);
            RefreshUI();
        }
        public void ShowDetailDesc(int itemId)
        {
            if (itemId == -1)
            {
                Get<Text>((int)Texts.Text_DetailDesc).text = "";
                return;
            }
            Data.Clue clue;
            _cludData.TryGetValue(itemId, out clue);
            string text = clue.description;

            Get<Text>((int)Texts.Text_DetailDesc).text = text;
        }
    }
}
