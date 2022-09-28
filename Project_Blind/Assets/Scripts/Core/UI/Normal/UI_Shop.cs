using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Shop : UI_Base
    {
        enum Images
        {
            Image_ItemIcon1,
            Image_ItemIcon2,
            Image_ItemIcon3,
            Image_ItemIcon4,
        }
        enum Texts
        {
            Text_CostItem1,
            Text_CostItem2,
            Text_CostItem3,
            Text_CostItem4,

            Text_MyMoney
        }
        enum Buttons
        {
            Button_BuyItem1,
            Button_BuyItem2,
            Button_BuyItem3,
            Button_BuyItem4,
        }
        Dictionary<int, Data.BagItem> dict;
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Text>(typeof(Texts));
            Bind<Button>(typeof(Buttons));
            dict = DataManager.Instance.BagItemDict;
            InitIcons();
            BindEvents();
            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.KeyInputEvents += HandleUIKeyInput;
        }
        private void InitIcons()
        {
            Get<Image>((int)Images.Image_ItemIcon1).sprite = ResourceManager.Instance.Load<Sprite>(dict[1].iconPath);
            Get<Image>((int)Images.Image_ItemIcon2).sprite = ResourceManager.Instance.Load<Sprite>(dict[2].iconPath);
            Get<Image>((int)Images.Image_ItemIcon3).sprite = ResourceManager.Instance.Load<Sprite>(dict[3].iconPath);
            Get<Image>((int)Images.Image_ItemIcon4).sprite = ResourceManager.Instance.Load<Sprite>(dict[4].iconPath);

            Get<Text>((int)Texts.Text_CostItem1).text = dict[1].cost.ToString();
            Get<Text>((int)Texts.Text_CostItem2).text = dict[2].cost.ToString();
            Get<Text>((int)Texts.Text_CostItem3).text = dict[3].cost.ToString();
            Get<Text>((int)Texts.Text_CostItem4).text = dict[4].cost.ToString();

            Get<Text>((int)Texts.Text_MyMoney).text = DataManager.Instance.GameData.money.ToString();
        }
        private void BindEvents()
        {
            Get<Button>((int)Buttons.Button_BuyItem1).gameObject.BindEvent(() => BuyItem(Define.BagItem.TestItem1));
            Get<Button>((int)Buttons.Button_BuyItem2).gameObject.BindEvent(() => BuyItem(Define.BagItem.TestItem2));
            Get<Button>((int)Buttons.Button_BuyItem3).gameObject.BindEvent(() => BuyItem(Define.BagItem.TestItem3));
            Get<Button>((int)Buttons.Button_BuyItem4).gameObject.BindEvent(() => BuyItem(Define.BagItem.TestItem4));
        }
        private void BuyItem(Define.BagItem itemId)
        {
            int cost = dict[(int)itemId].cost;
            bool result = DataManager.Instance.SubMoney(cost);
            if (result == false) return;
            DataManager.Instance.AddBagItem(itemId);
            Get<Text>((int)Texts.Text_MyMoney).text = DataManager.Instance.GameData.money.ToString();
        }
        private void HandleUIKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
                UIManager.Instance.CloseNormalUI(this);
                return;
            }
        }
        private void OnDestroy()
        {
            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
        }
    }
}


