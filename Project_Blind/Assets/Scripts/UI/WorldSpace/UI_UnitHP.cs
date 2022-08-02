using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_UnitHP : UI_WorldSpace
    {
        private UnitHP _unitHP;
        private CircleCollider2D _colider;
        public UnitHP HP
        {
            get { return _unitHP; }
            set { _unitHP = value; }
        }
        enum Images
        {
            Image_HP_Background,
            Image_CurrHP,
        }
        public override void Init()
        {
            base.Init();
            Bind<Image>(typeof(Images));
            InitCollider();
        }
        protected override void Start()
        {
            base.Start();
            InitEvents();
            gameObject.GetComponent<Canvas>().enabled = false;
        }
        private void InitEvents()
        {
            _unitHP.RefreshHpUI += OnHpChanged;
        }
        private void InitCollider()
        {
            _colider = Util.GetOrAddComponent<CircleCollider2D>(gameObject);
            _colider.isTrigger = true;
            _colider.offset = new Vector3(0, -60f, 0);
            _colider.radius = 200.0f;
        }
        private void OnHpChanged(float currHP, float maxHP)
        {
            RefreshHPUI();
        }
        private void RefreshHPUI()
        {
            Get<Image>((int)Images.Image_CurrHP).transform.localScale =
                new Vector3(_unitHP.GetHP() / _unitHP.GetMaxHP(), 1f, 1f);
        }
        public void Reverse()
        {
            Image background = Get<Image>((int)Images.Image_HP_Background);
            background.transform.Rotate(0, 180, 0);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null)
                return;
            gameObject.GetComponent<Canvas>().enabled = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null)
                return;
            gameObject.GetComponent<Canvas>().enabled = false;
        }
    }
}

