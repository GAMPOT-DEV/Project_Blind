using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Clue_Item : UI_Base
    {
        private UI_Clue _owner;
        Dictionary<int, Data.Clue> _cludData;
        public Data.Clue Clue { get; private set; }

        [SerializeField] private Sprite Panel_Clicked;
        [SerializeField] private Sprite Panel_Non_Clicked;
        [SerializeField] private Sprite Button_Clicked;
        [SerializeField] private Sprite Button_Non_Clicked;
        enum Images
        {
            Image_ItemIcon,
            Image_Panel,
            Image_Background,

            Image_Line_Up,
            Image_Line_Down,
            Image_Z,
        }
        enum Texts
        {
            Text_ClueName,
            Text_ClueDesc
        }
        enum GameObjects
        {
            Go_ClueDescAndLineUp,
        }
        public int ItemId { get; private set; }
        private int _index;
        private bool _pushZButton = false;

        private float UP_DIST;
        private float DOWN_DIST;

        private Coroutine _coroutine = null;
        int _coroutineState = 0;
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Text>(typeof(Texts));
            Bind<GameObject>(typeof(GameObjects));
            _cludData = DataManager.Instance.ClueDict;
            Get<Image>((int)Images.Image_ItemIcon).gameObject.BindEvent(PushItemIcon, Define.UIEvent.Click);

            UP_DIST = 500f;
            DOWN_DIST = 130f;
        }
        private void OnEnable()
        {
            if (_coroutineState == -1)
            {
                StartCoroutine(CoGoDown());
            }
            else if (_coroutineState == 1)
            {
                StartCoroutine(CoGoUp());
            }
            //Get<GameObject>((int)GameObjects.Go_ClueDescAndLineUp).transform.position = Get<Image>((int)Images.Image_Line_Down).transform.position + Vector3.up * DOWN_DIST;
            //SetNonClickedState();
            //Get<Image>((int)Images.Image_Z).sprite = Button_Non_Clicked;
            //_pushZButton = false;
            //Get<Text>((int)Texts.Text_ClueDesc).gameObject.SetActive(false);
            //Get<Text>((int)Texts.Text_ClueName).gameObject.SetActive(true);
            //_coroutine = null;
        }
        public void SetItem(int itemId, int index, UI_Clue owner)
        {
            _owner = owner;
            ItemId = itemId;
            _index = index;
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

            Get<Text>((int)Texts.Text_ClueName).text = Clue.name;
            Get<Text>((int)Texts.Text_ClueDesc).text = Clue.description;
            Get<Text>((int)Texts.Text_ClueDesc).gameObject.SetActive(false);

            SetNonClickedState();
        }
        public void PushItemIcon()
        {
            _owner.PushClueItem(_index);
        }
        public void SetClickedState()
        {
            UIManager.Instance.KeyInputEvents -= CheckZInput;
            UIManager.Instance.KeyInputEvents += CheckZInput;

            Get<Image>((int)Images.Image_Panel).gameObject.SetActive(false);
            Get<Image>((int)Images.Image_Background).sprite = Panel_Clicked;
        }
        public void SetNonClickedState()
        {
            UIManager.Instance.KeyInputEvents -= CheckZInput;

            Get<Image>((int)Images.Image_Panel).gameObject.SetActive(true);
            Get<Image>((int)Images.Image_Background).sprite = Panel_Non_Clicked;
        }
        private void CheckZInput()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (_pushZButton)
                {
                    if (_coroutine != null)
                        return;

                    _pushZButton = false;
                    Get<Image>((int)Images.Image_Z).sprite = Button_Non_Clicked;

                    Get<Text>((int)Texts.Text_ClueDesc).gameObject.SetActive(false);
                    Get<Text>((int)Texts.Text_ClueName).gameObject.SetActive(true);

                    _coroutine = StartCoroutine(CoGoDown());
                }
                else
                {
                    if (_coroutine != null)
                        return;

                    _pushZButton = true;
                    Get<Image>((int)Images.Image_Z).sprite = Button_Clicked;

                    Get<Text>((int)Texts.Text_ClueDesc).gameObject.SetActive(true);
                    Get<Text>((int)Texts.Text_ClueName).gameObject.SetActive(false);

                    _coroutine = StartCoroutine(CoGoUp());
                }
            }
        }
        public void CloseUI()
        {
            UIManager.Instance.KeyInputEvents -= CheckZInput;
        }
        IEnumerator CoGoUp()
        {
            _coroutineState = 1;
            while (true)
            {
                if (Get<Image>((int)Images.Image_Line_Down).transform.position.y + UP_DIST <= Get<GameObject>((int)GameObjects.Go_ClueDescAndLineUp).transform.position.y)
                {
                    Get<GameObject>((int)GameObjects.Go_ClueDescAndLineUp).transform.position = Get<Image>((int)Images.Image_Line_Down).transform.position + Vector3.up * UP_DIST;
                    _coroutine = null;
                    _coroutineState = 0;
                    break;
                }
                Get<GameObject>((int)GameObjects.Go_ClueDescAndLineUp).transform.position += Vector3.up * 800f * Time.unscaledDeltaTime;
                yield return null;
            }
        }
        IEnumerator CoGoDown()
        {
            _coroutineState = -1;
            while (true)
            {
                if (Get<Image>((int)Images.Image_Line_Down).transform.position.y + DOWN_DIST >= Get<GameObject>((int)GameObjects.Go_ClueDescAndLineUp).transform.position.y)
                {
                    Get<GameObject>((int)GameObjects.Go_ClueDescAndLineUp).transform.position = Get<Image>((int)Images.Image_Line_Down).transform.position + Vector3.up * DOWN_DIST;
                    _coroutine = null;
                    _coroutineState = 0;
                    break;
                }
                Get<GameObject>((int)GameObjects.Go_ClueDescAndLineUp).transform.position -= Vector3.up * 800f * Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}

