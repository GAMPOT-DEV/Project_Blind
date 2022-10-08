using UnityEngine;

namespace Blind
{
    public abstract class ParingEffect : MonoBehaviour
    {
        /// <summary>
        /// 적 공격이 패링이 가능한 공격인지 판별하는 함수입니다
        /// </summary>
        public abstract void GetParing();

        /// <summary>
        /// 적 공격의 종류의 따라 상대가 받을 디버프 정의하는 함수입니다.
        /// </summary>
        public abstract void EnemyDebuff();

    }
}