using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Talisman_Item : UI_Base
    {
        [SerializeField] private Sprite EquipButton;
        [SerializeField] private Sprite UnequipButton;
        private UI_Talisman _owner;
        Dictionary<int, Data.TalismanItem> _talismanData;
        public Data.TalismanItem Talisman { get; private set; }
        enum Images
        {
            Image_ItemIcon
        }
        enum Texts
        {
            Text_ItemName,
            Text_ItemDesc,
            Text_ItemEffect
        }
        enum Buttons
        {
            Button_Equip,
        }
        public int ItemId { get; private set; }
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Text>(typeof(Texts));
            Bind<Button>(typeof(Buttons));
            _talismanData = DataManager.Instance.TalismanItemDict;
            Get<Button>((int)Buttons.Button_Equip).gameObject.BindEvent(PushUnequipButton);
        }
        public void SetItem(int itemId, int index, UI_Talisman owner)
        {
            _owner = owner;
            ItemId = itemId;
            Data.TalismanItem talisman;
            _talismanData.TryGetValue(itemId, out talisman);
            Talisman = talisman;
            if (Talisman == null)
            {
                Get<Image>((int)Images.Image_ItemIcon).sprite = null;
            }
            else
            {
                Sprite sprite = ResourceManager.Instance.Load<Sprite>(Talisman.iconPath);
                Get<Image>((int)Images.Image_ItemIcon).sprite = sprite;
            }

            Get<Text>((int)Texts.Text_ItemName).text = Talisman.name;
            Get<Text>((int)Texts.Text_ItemDesc).text = Talisman.description;

            TalismanInfo info;
            DataManager.Instance.GameData.TalismanInfoById.TryGetValue(ItemId, out info);
            if (info.equiped == true)
            {
                Get<Button>((int)Buttons.Button_Equip).image.sprite = UnequipButton;
            }
            else
            {
                Get<Button>((int)Buttons.Button_Equip).image.sprite = EquipButton;
            }
        }
        private void PushUnequipButton()
        {
            SoundManager.Instance.Play("Select");
            bool result = DataManager.Instance.EquipOrUnequipTalisman((Define.TalismanItem)ItemId);
            if (result == false) return;
            TalismanInfo info;
            DataManager.Instance.GameData.TalismanInfoById.TryGetValue(ItemId, out info);
            if (info.equiped == true)
            {
                _owner.EquipTalisman(ItemId);
                Get<Button>((int)Buttons.Button_Equip).image.sprite = UnequipButton;
            }
            else
            {
                _owner.UnequipTalisman(ItemId);
                Get<Button>((int)Buttons.Button_Equip).image.sprite = EquipButton;
            }
        }
    }
}
