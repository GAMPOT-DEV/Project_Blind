using UnityEngine;

namespace Blind
{
    public class BossAttackPattern<TMonoBehaviour> : MonoBehaviour
        where TMonoBehaviour : MonoBehaviour

    {
        protected TMonoBehaviour _gameobject;

        public void Initialise(TMonoBehaviour gameobject)
        {
            _gameobject = gameobject;
        }

        public virtual Coroutine AttackPattern()
        {
            return null;
        }
    }
}