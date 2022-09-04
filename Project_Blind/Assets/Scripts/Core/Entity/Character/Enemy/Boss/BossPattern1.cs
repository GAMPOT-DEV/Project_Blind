using System.Collections;
using UnityEngine;

namespace Blind
{
    public class BossPattern1 : BossAttackPattern<FirstBossEnemy>
    {
        private GameObject player;
        [SerializeField] private GameObject rangeObject;
        private BoxCollider2D _collider;
        private Transform RealAttackPostion;
        private bool isAttackStart;

        private void Init()
        {
            player = GameObject.Find("Player(animation)");
            _collider = rangeObject.GetComponent<BoxCollider2D>();
            rangeObject.transform.position = player.transform.position;
            RealAttackPostion = this.transform;
            isAttackStart = false;
        }

        Vector2 Return_RandomPosion()
        {
            Vector2 originposion = rangeObject.transform.position;
            float random_x = _collider.bounds.size.x;
            float random_y = _collider.bounds.size.y;
            Debug.Log(random_y);
            random_x = Random.Range((random_x / 2) * -1, random_x / 2);
            random_y = Random.Range((random_y / 2), random_y / 2); 
            Vector2 randomPosiotion = new Vector2(random_x, random_y);
            Vector2 RespawnPostion = randomPosiotion + originposion;
            return RespawnPostion;
        }
        public override void AttackPattern()
        {
            Init();
            int AttackRange = RandomRange();
            int RealAttack = RandomRange(AttackRange);
            StartCoroutine(SettingAttack(AttackRange, RealAttack));
        }

        IEnumerator SettingAttack(int AttackRange, int RealAttack)
        {
            int n = 0;
            while (n!= AttackRange)
            {
                yield return new WaitForSeconds(0.5f);
                Vector2 postion = Return_RandomPosion();
                if (n == RealAttack)
                {
                    RealAttackPostion.position = postion;
                }
                var wave = ResourceManager.Instance.Instantiate("MapObjects/Wave/WaveSense 1").GetComponent<WaveSense>();
                wave.transform.position = postion;
                wave.StartSpread();
                n++;
            }
            StartCoroutine(StartAttack());
        }

        IEnumerator StartAttack()
        {
            yield return new WaitForSeconds(1f);
            var bossHand = ResourceManager.Instance.Instantiate("Enemy/Boss/BossHand").GetComponent<BossHand>();

            bossHand.transform.position = RealAttackPostion.position;
            Transform currentPlayerPostion = player.transform;
            bossHand.GetTransform(RealAttackPostion, currentPlayerPostion);
        }

        private int RandomRange(int end = 5)
        {
            return Random.Range(2, end);
        }
    }
}