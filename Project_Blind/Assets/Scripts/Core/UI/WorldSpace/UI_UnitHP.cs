using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_UnitHP : UI_WorldSpace
    {
        private UnitHP _unitHP;
        private CircleCollider2D _collider;
        private bool _checkPlayer = false;
        private int _mask;
        private PlayerCharacter _player;

        // 플레이어가 몬스터를 볼 수 있는 범위
        private float _dist = 10.0f;

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

            // 레이어 설정
            _mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Floor");
        }
        private void InitEvents()
        {
            _unitHP.RefreshHpUI += OnHpChanged;
        }
        private void InitCollider()
        {
            _collider = Util.GetOrAddComponent<CircleCollider2D>(gameObject);
            _collider.isTrigger = true;
            _collider.offset = new Vector3(0, -60f, 0);
            _collider.radius = 400.0f;
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

            // 범위 안에 플레이어가 들어오면 레이캐스트를 써서 몬스터와 캐릭터 중간에 뭐가 있는지 확인
            _checkPlayer = true;
            _player = collision.gameObject.GetComponent<PlayerCharacter>();

            StartCoroutine(CoCheckPlayer());
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null)
                return;

            // 플레이어가 범위에서 벗어나면 탐색 종료
            _checkPlayer = false;
            _player = null;
        }
        IEnumerator CoCheckPlayer()
        {
            while (true)
            {
                if (!_checkPlayer)
                    break;
                Vector3 myPos = Owner.transform.position;
                Vector3 playerPos = _player.transform.position;
                Vector3 dir = (playerPos - myPos).normalized;
                // 내 위치에서 플레이어 위치로 레이캐스트
                var result = Physics2D.Raycast(myPos, dir, _dist, _mask);
                Debug.DrawRay(myPos, dir * _dist, Color.red, 0.1f);

                //레이캐스트에 충돌이 됐을 때
                if (result.collider != null)
                {
                    // 처음으로 충돌한게 플레이어라면
                    if (result.collider.gameObject.GetComponent<PlayerCharacter>() != null)
                    {
                        // UI 보이게함
                        gameObject.GetComponent<Canvas>().enabled = true;
                    }
                    else
                    {
                        gameObject.GetComponent<Canvas>().enabled = false;
                    }
                }
                else
                {
                    gameObject.GetComponent<Canvas>().enabled = false;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

