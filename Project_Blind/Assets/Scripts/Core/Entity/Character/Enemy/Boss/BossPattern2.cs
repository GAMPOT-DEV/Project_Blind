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
        }
        public override void AttackPattern()
        {
            Debug.Log("dd");
            Init();
            var bossHand = ResourceManager.Instance.Instantiate("Enemy/Boss/BossHand").GetComponent<BossHand>();
            int range = RandomRange();
            bossHand.GetFacing(range);
            if (range == 0)
            {
                bossHand.transform.position = _floorStart.position;
                bossHand.GetTransform(_floorStart, _floorEnd);
                
            }
            else
            {
                Debug.Log("DD");
                bossHand.transform.position = _floorEnd.position;
                bossHand.GetTransform(_floorEnd,_floorStart);
            }

            bossHand.isMove = true;
        }
        
        private int RandomRange()
        {
            return Random.Range(0, 2);
        }
    }
}