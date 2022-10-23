using System.Linq.Expressions;
using System.Collections;
using UnityEngine;
namespace Blind
{
    public class BossPattern2: BossAttackPattern<FirstBossEnemy>
    {
        private CapsuleCollider2D _collider2D;
        private Rigidbody2D _rigidbody;
        private Transform _floorStart;
        private Transform _floorEnd;
        private void Init()
        {
            _floorStart = _gameobject._floorStart;
            _floorEnd = _gameobject._floorEnd;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z * 80);
            Debug.Log(transform.rotation.z);
        }
        public override Coroutine AttackPattern()
        {
            Init();
            var bossHand = ResourceManager.Instance.Instantiate("Enemy/Boss/BossHand").GetComponent<BossHand>();


            bossHand.CheckBossPattern(false);
            int range = RandomRange();
            SoundManager.Instance.Play("장산범/바닥긁기공격", Define.Sound.Effect);
            return StartCoroutine( StartAttack(range, bossHand));
        }

        IEnumerator StartAttack(int range, BossHand bossHand)
        {
            if (range == 0)
            {
                bossHand.GetComponent<SpriteRenderer>().flipX = false;
                bossHand.transform.position = _floorStart.position;
                bossHand.GetTransform(_floorStart.position, _floorEnd.position, false);
                bossHand.GetFacing(true);
            }
            else
            {
                Debug.Log("실행됨ㅇㅇ");
                bossHand.GetComponent<SpriteRenderer>().flipX = true;
                bossHand.transform.position = _floorEnd.position;
                bossHand.GetTransform(_floorEnd.position,_floorStart.position, false);
                bossHand.GetFacing(false);
            }
            yield return new WaitForSeconds(3f);
        }
        
        private int RandomRange()
        {
            return Random.Range(0, 2);
        }
    }
}