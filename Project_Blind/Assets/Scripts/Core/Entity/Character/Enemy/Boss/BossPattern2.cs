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
            Init();
            var bossHand = ResourceManager.Instance.Instantiate("Enemy/Boss/BossHand").GetComponent<BossHand>();
            int range = RandomRange();
            if (range == 0)
            {
                bossHand.transform.position = _floorStart.position;
                bossHand.GetTransform(_floorStart.position, _floorEnd.position);
                bossHand.GetFacing(false);
            }
            else
            {
                bossHand.transform.position = _floorEnd.position;
                bossHand.GetTransform(_floorEnd.position,_floorStart.position);
                bossHand.GetFacing(true);
            }
        }
        
        private int RandomRange()
        {
            return Random.Range(0, 2);
        }
    }
}