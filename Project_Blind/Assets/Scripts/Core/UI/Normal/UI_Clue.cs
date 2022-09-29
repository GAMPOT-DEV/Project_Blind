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
        }
        enum Texts
        {

        }
        Dictionary<int, Data.Clue> _cludData;
        public static int Size { get; set; }
        private int _currIdx = -1;
        public List<UI_Clue_Item> Items { get; } = new List<UI_Clue_Item>();
        GameObject grid;
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            //Bind<Text>(typeof(Texts));

            _cludData = DataManager.Instance.ClueDict;

            Items.Clear();

            // 일단 아이템 슬롯들을 전부 날려준 뒤
            grid = GetComponentInChildren<GridLayoutGroup>().gameObject;
            foreach (Transform child in grid.transform)
                Destroy(child.gameObject);

            // SIZE만큼 슬롯들을 만들어서 Items에서 관리하도록 한다.
            Size = DataManager.Instance.GameData.clueInfos.Count;
            for (int i = 0; i < Size; i++)
            {
                GameObject go = ResourceManager.Instance.Instantiate("UI/Normal/UI_Clue_Item", grid.transform);
                UI_Clue_Item item = go.GetOrAddComponent<UI_Clue_Item>();
                Items.Add(item);
            }

            // UI를 데이터 정보에 맞게 갱신해준다.
            RefreshUI();
        }
        // 현재 가지고 있는 단서들의 정보를 이용해서 UI를 갱신해준다.
        public void RefreshUI()
        {
            Size = DataManager.Instance.GameData.clueInfos.Count;
            grid.GetComponent<RectTransform>().sizeDelta = new Vector2(500f * (float)Size - 1800f, 770f);
            for(int i = 0; i < Size; i++)
            {
                int itemId = -1;

                ClueInfo info = null;
                DataManager.Instance.GameData.ClueInfoBySlot.TryGetValue(i, out info);
                if (info != null)
                    itemId = info.itemId;

                // 각각의 슬롯에 아이템 ID를 넘겨줘서 갱신한다.
                // 위에서 찾지 못하면 ID = -1이고 ID가 -1인 아이템은 없기 때문에 
                // 자동으로 null로 할당된다.
                Items[i].SetItem(itemId, i, this);
            }
        }
        private void PushTestImage(Define.ClueItem itemEnum)
        {
            // 게임데이터에 아이템을 추가를 시도한다.
            bool result = DataManager.Instance.AddClueItem(itemEnum);
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
            if (_currIdx != -1)
                Items[_currIdx].CloseUI();
            _currIdx = -1;

            DataManager.Instance.ClearClueData();
            Items.Clear();
            // 단서 슬롯들을 전부 날려주고 UI를 새로고침해준다.
            foreach (Transform child in grid.transform)
                Destroy(child.gameObject);

            //ShowDetailDesc(-1);
            RefreshUI();
            
        }
        public void PushClueItem(int idx)
        {
            if (_currIdx != -1)
            {
                Items[_currIdx].SetNonClickedState();
            }
            _currIdx = idx;
            Items[_currIdx].SetClickedState();
        }
        public void CloseUI()
        {
            if (_currIdx == -1) return;
            Items[_currIdx].CloseUI();
        }
    }
}
