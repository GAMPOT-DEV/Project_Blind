using System;

namespace Blind
{
    /// <summary>
    /// 게임 매니저 안에 들어갈 오브젝트들이 상속받는 인터페이스입니다.
    /// Update,FixedUpdate등 분리하면 비용이 많이 드는 것들을 합치기 위함입니다.
    /// </summary>
    public interface IGameManagerObj
    {
        public void OnFixedUpdate()
        {
            return;
        }
    }
}