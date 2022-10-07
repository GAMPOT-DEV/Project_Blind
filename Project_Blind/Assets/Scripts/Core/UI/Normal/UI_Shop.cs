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
            Image_Item1,
            Image_Item2,
            Image_Item3,
            Image_Item4,
            Image_Item5,
            Image_Item6,
        }
        enum Texts
        {
            Text_MyMoney,

            Text_Item1Name,
            Text_Item1Cost,
            Text_Item1Disc,

            Text_Item2Name,
            Text_Item2Cost,
            Text_Item2Disc,

            Text_Item3Name,
            Text_Item3Cost,
            Text_Item3Disc,

            Text_Item4Name,
            Text_Item4Cost,
            Text_Item4Disc,

            Text_Item5Name,
            Text_Item5Cost,
            Text_Item5Disc,

            Text_Item6Name,
            Text_Item6Cost,
            Text_Item6Disc,
        }
        enum Buttons
        {
            Button_Buy1,
            Button_Buy2,
            Button_Buy3,
            Button_Buy4,
            Button_Buy5,
            Button_Buy6,
        }
        Dictionary<int, Data.BagItem> bagDict;
        Dictionary<int, Data.TalismanItem> talismanDict;
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Text>(typeof(Texts));
            Bind<Button>(typeof(Buttons));
            bagDict = DataManager.Instance.BagItemDict;
            talismanDict = DataManager.Instance.TalismanItemDict;
            InitIcons();
            BindEvents();
            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.KeyInputEvents += HandleUIKeyInput;
        }
        private void InitIcons()
        {
            Get<Image>((int)Images.Image_Item1).sprite = ResourceManager.Instance.Load<Sprite>(bagDict[(int)Define.BagItem.WaveStick].iconPath);
            Get<Image>((int)Images.Image_Item2).sprite = ResourceManager.Instance.Load<Sprite>(bagDict[(int)Define.BagItem.Potion].iconPath);

            Get<Image>((int)Images.Image_Item3).sprite = ResourceManager.Instance.Load<Sprite>(talismanDict[(int)Define.TalismanItem.Talisman_Green].iconPath);
            Get<Image>((int)Images.Image_Item4).sprite = ResourceManager.Instance.Load<Sprite>(talismanDict[(int)Define.TalismanItem.Talisman_Red].iconPath);
            Get<Image>((int)Images.Image_Item5).sprite = ResourceManager.Instance.Load<Sprite>(talismanDict[(int)Define.TalismanItem.Talisman_Purple].iconPath);
            Get<Image>((int)Images.Image_Item6).sprite = ResourceManager.Instance.Load<Sprite>(talismanDict[(int)Define.TalismanItem.Talisman_Blue].iconPath);

            Get<Text>((int)Texts.Text_Item1Name).text = bagDict[(int)Define.BagItem.WaveStick].name;
            Get<Text>((int)Texts.Text_Item2Name).text = bagDict[(int)Define.BagItem.Potion].name;

            Get<Text>((int)Texts.Text_Item1Cost).text = "영혼" + bagDict[(int)Define.BagItem.WaveStick].cost.ToString();
            Get<Text>((int)Texts.Text_Item2Cost).text = "영혼" + bagDict[(int)Define.BagItem.Potion].cost.ToString();

            Get<Text>((int)Texts.Text_Item1Disc).text = bagDict[(int)Define.BagItem.WaveStick].shortDescription;
            Get<Text>((int)Texts.Text_Item2Disc).text = bagDict[(int)Define.BagItem.Potion].shortDescription;

            Get<Text>((int)Texts.Text_Item3Name).text = talismanDict[(int)Define.TalismanItem.Talisman_Green].name;
            Get<Text>((int)Texts.Text_Item4Name).text = talismanDict[(int)Define.TalismanItem.Talisman_Red].name;
            Get<Text>((int)Texts.Text_Item5Name).text = talismanDict[(int)Define.TalismanItem.Talisman_Purple].name;
            Get<Text>((int)Texts.Text_Item6Name).text = talismanDict[(int)Define.TalismanItem.Talisman_Blue].name;

            Get<Text>((int)Texts.Text_Item3Cost).text = "영혼" + talismanDict[(int)Define.TalismanItem.Talisman_Green].cost.ToString();
            Get<Text>((int)Texts.Text_Item4Cost).text = "영혼" + talismanDict[(int)Define.TalismanItem.Talisman_Red].cost.ToString();
            Get<Text>((int)Texts.Text_Item5Cost).text = "영혼" + talismanDict[(int)Define.TalismanItem.Talisman_Purple].cost.ToString();
            Get<Text>((int)Texts.Text_Item6Cost).text = "영혼" + talismanDict[(int)Define.TalismanItem.Talisman_Blue].cost.ToString();

            Get<Text>((int)Texts.Text_Item3Disc).text = talismanDict[(int)Define.TalismanItem.Talisman_Green].description;
            Get<Text>((int)Texts.Text_Item4Disc).text = talismanDict[(int)Define.TalismanItem.Talisman_Red].description;
            Get<Text>((int)Texts.Text_Item5Disc).text = talismanDict[(int)Define.TalismanItem.Talisman_Purple].description;
            Get<Text>((int)Texts.Text_Item6Disc).text = talismanDict[(int)Define.TalismanItem.Talisman_Blue].description;

            Get<Text>((int)Texts.Text_MyMoney).text = DataManager.Instance.GameData.money.ToString();
        }
        private void BindEvents()
        {
            Get<Button>((int)Buttons.Button_Buy1).gameObject.BindEvent(() => BuyBagItem(Define.BagItem.WaveStick));
            Get<Button>((int)Buttons.Button_Buy2).gameObject.BindEvent(() => BuyBagItem(Define.BagItem.Potion));

            Get<Button>((int)Buttons.Button_Buy3).gameObject.BindEvent(() => BuyTalismanItem(Define.TalismanItem.Talisman_Green));
            Get<Button>((int)Buttons.Button_Buy4).gameObject.BindEvent(() => BuyTalismanItem(Define.TalismanItem.Talisman_Red));
            Get<Button>((int)Buttons.Button_Buy5).gameObject.BindEvent(() => BuyTalismanItem(Define.TalismanItem.Talisman_Purple));
            Get<Button>((int)Buttons.Button_Buy6).gameObject.BindEvent(() => BuyTalismanItem(Define.TalismanItem.Talisman_Blue));
        }
        private void BuyBagItem(Define.BagItem itemId)
        {
            SoundManager.Instance.Play("Select");
            int cost = bagDict[(int)itemId].cost;
            bool result = DataManager.Instance.SubMoney(cost);
            if (result == false) return;
            DataManager.Instance.AddBagItem(itemId);
            Get<Text>((int)Texts.Text_MyMoney).text = DataManager.Instance.GameData.money.ToString();
        }
        private void BuyTalismanItem(Define.TalismanItem itemId)
        {
            SoundManager.Instance.Play("Select");
            int cost = talismanDict[(int)itemId].cost;
            if (DataManager.Instance.HaveTalismanItem(itemId)) return;
            bool result = DataManager.Instance.SubMoney(cost);
            if (result == false) return;
            DataManager.Instance.AddTalismanItem(itemId);
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


