using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 데미지를 받을 수 있는 모든 클래스의 부모 인터페이스입니다. ( ex : 유닛, 공격 횟수에 따라 열리는 오브젝트 )
    /// </summary>
    public interface IDamageable<T>
    {
        public void GetDamage(T damage);
        public void GetHeal(T heal);
        public T GetHP();
    }
}